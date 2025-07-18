﻿using HalconDotNet;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.ViewModels.Pages;

namespace WheelRecognitionSystem.Public
{
    /// <summary>
    /// 图像处理助手
    /// </summary>
    public class ImageProcessingHelper
    {
        /// <summary>
        /// 剪裁图像
        /// </summary>
        /// <param name="image"></param>
        /// <param name="croppedImage"></param>
        /// <returns></returns>
        public static void Cropping(HObject image, out HObject croppedImage)
        {
            HOperatorSet.GetImageSize(image, out HTuple width, out HTuple height);
            int x1 = 0;
            int y1 = 100;
            double w = width.D;
            int TargetWidth = (int)(w - y1 * 2);
            int TargetHeight = (int)height.D;
            HOperatorSet.CropPart(image, out croppedImage, x1, y1, TargetWidth, TargetHeight);
        }
        /// <summary>
        /// 获取灰度图 返回一个新对象
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static HObject RGBTransGray(HObject image)
        {
            //彩色图需转成灰度图
            HOperatorSet.CountChannels(image, out HTuple Channels);

            if (Channels.I == 3)
            {
                HOperatorSet.Decompose3(image, out HObject image1, out HObject image2, out HObject image3);
                image2.Dispose();
                image3.Dispose();
                Channels.Dispose();
                return image1.Clone();
            }
            else
                return image.Clone();

        }

        public static void SafeHalconDispose<T>(T obj) where T : class, IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null; // 关键：解除引用使GC可回收
            }
        }

        // 安全克隆方法
        public static HObject CloneImageSafely(HObject source)
        {
            return (source != null && source.IsInitialized()) ? source.Clone() : null;
        }

        /// <summary>
        /// 定位轮毂
        /// </summary>
        /// <param name="image">源图像</param>
        /// <param name="minThreshold">最小阈值</param>
        /// <param name="maxThreshold">最大阈值</param>
        /// <returns>定位到的轮毂图像和轮毂轮廓</returns>
        public static PositioningWheelResultModel PositioningWheel(HObject imageSource, int minThreshold, int maxThreshold, int minRadius, bool isConfirmRadius = true)
        {
            PositioningWheelResultModel resultModel = new PositioningWheelResultModel();
            HObject image = null;
            try
            {
                image = CloneImageSafely(imageSource);
                //全图灰度
                resultModel.FullFigureGary = (float)GetIntensity(image);


                HOperatorSet.Threshold(image, out HObject region, minThreshold, maxThreshold);
                HOperatorSet.Connection(region, out HObject connectedRegions);
                HOperatorSet.FillUp(connectedRegions, out HObject regionFillUp);
                HOperatorSet.SelectShapeStd(regionFillUp, out HObject relectedRegions, "max_area", 70);
                HOperatorSet.InnerCircle(relectedRegions, out HTuple row, out HTuple column, out HTuple radius);


                SafeHalconDispose(region);
                SafeHalconDispose(connectedRegions);
                SafeHalconDispose(regionFillUp);
                SafeHalconDispose(relectedRegions);


                if (row.Length != 0) //存在轮毂
                {
                    //确认半径范围
                    if (isConfirmRadius && radius < minRadius)
                    {
                        //半径不符合
                    }
                    else
                    {
                        //获取内圆 通过找圆心的方式
                        HOperatorSet.GetImageSize(image, out HTuple width, out HTuple height);
                        HOperatorSet.CreateMetrologyModel(out HTuple MetrologyCircleHandle);
                        HOperatorSet.SetMetrologyModelImageSize(MetrologyCircleHandle, width, height);
                        HTuple H1 = new HTuple();
                        HTuple H2 = new HTuple();
                        HOperatorSet.AddMetrologyObjectCircleMeasure(MetrologyCircleHandle, row, column, radius + 10, 160, 3, 1, 30, H1, H2, out HTuple CircleIndex);
                        HOperatorSet.SetMetrologyObjectParam(MetrologyCircleHandle, CircleIndex, "num_instances", 1);
                        HOperatorSet.SetMetrologyObjectParam(MetrologyCircleHandle, CircleIndex, "min_score", 0.1);
                        HOperatorSet.ApplyMetrologyModel(image, MetrologyCircleHandle);

                        HOperatorSet.GetMetrologyObjectResult(MetrologyCircleHandle, CircleIndex, "all", "result_type", "all_param", out HTuple hv_Parameter);
                        HOperatorSet.GetMetrologyObjectMeasures(out HObject ho_Contours, MetrologyCircleHandle, CircleIndex, "all", out HTuple hv_Row, out HTuple hv_Column); //工具环
                        HOperatorSet.GetMetrologyObjectResultContour(out HObject ho_Contour, MetrologyCircleHandle, CircleIndex, "all", 1.5); //环
                        if (hv_Parameter.Length > 0)
                        {
                            row = hv_Parameter.TupleSelect(0);
                            column = hv_Parameter.TupleSelect(1);
                            radius = hv_Parameter.TupleSelect(2);
                        }

                        HOperatorSet.ClearMetrologyModel(MetrologyCircleHandle);
                        SafeHalconDispose(width);
                        SafeHalconDispose(height);
                        SafeHalconDispose(MetrologyCircleHandle);
                        SafeHalconDispose(H1);
                        SafeHalconDispose(H2);
                        SafeHalconDispose(CircleIndex);
                        SafeHalconDispose(hv_Parameter);
                        SafeHalconDispose(ho_Contours);
                        SafeHalconDispose(hv_Row);
                        SafeHalconDispose(hv_Column);
                        SafeHalconDispose(ho_Contour);



                        resultModel.CenterRow = row.Clone();
                        resultModel.CenterColumn = column.Clone();
                        resultModel.Radius = radius.Clone();

                        HOperatorSet.GenCircle(out HObject reducedCircle, row, column, radius);
                        HOperatorSet.GenCircleContourXld(out HObject wheelContour, row, column, radius, 0, (new HTuple(360)).TupleRad(), "positive", 1.0);
                        HOperatorSet.ReduceDomain(image, reducedCircle, out HObject wheelImage);
                        HOperatorSet.Intensity(wheelImage, wheelImage, out HTuple mean, out HTuple deviation); //圈内灰度
                        resultModel.InnerCircleMean = (float)(mean.D);
                        resultModel.WheelImage = wheelImage.Clone();
                        resultModel.WheelContour = wheelContour.Clone();

                        SafeHalconDispose(reducedCircle);
                        SafeHalconDispose(wheelImage);
                        SafeHalconDispose(mean);
                        SafeHalconDispose(deviation);
                        SafeHalconDispose(wheelContour);


                    }
                }
                SafeHalconDispose(row);
                SafeHalconDispose(column);
                SafeHalconDispose(radius);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"PositioningWheelResultModel:{ex.Message}");
            }
            SafeHalconDispose(image);
            return resultModel;
        }

        /// <summary>
        /// 获取图片平均灰度值
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static double GetIntensity(HObject imageSource)
        {
            HObject grayImage = CloneImageSafely(imageSource);
            //全图灰度
            HOperatorSet.Intensity(grayImage, grayImage, out HTuple mean, out HTuple deviation);
            float fullFigureGary = (float)(mean.D);
            SafeHalconDispose(grayImage);
            SafeHalconDispose(mean);
            SafeHalconDispose(deviation);
            return fullFigureGary;
        }



        /// <summary>
        /// 轮毂识别算法
        /// </summary>
        /// <param name="image">源图像</param>
        /// <param name="templateDatas">模板数据</param>
        /// <param name="angleStart">起始角度</param>
        /// <param name="angleExtent">角度范围</param>
        /// <param name="minSimilarity">最小相似度</param>
        /// <returns>识别结果</returns>
        //public static RecognitionResultModel WheelRecognitionAlgorithm(HObject image, TemplateDatasModel templateDatas, double angleStart, double angleExtent, double minSimilarity)
        //{
        //    RecognitionResultModel activeIdentifyData = new RecognitionResultModel();
        //    RecognitionResultModel notActiveIdentifyData = new RecognitionResultModel();

        //    List<HTuple> rows = new List<HTuple>();
        //    List<HTuple> columns = new List<HTuple>();
        //    List<HTuple> angles = new List<HTuple>();

        //    List<HTuple> rowsN = new List<HTuple>();
        //    List<HTuple> columnsN = new List<HTuple>();
        //    List<HTuple> anglesN = new List<HTuple>();
        //    //活跃模板匹配，并将结果放入对应的匹配结果列表
        //    if (templateDatas.ActiveTemplates.Count > 0)
        //    {
        //        for (int i = 0; i < templateDatas.ActiveTemplates.Count; i++)
        //        {
        //            HOperatorSet.FindNccModel(image, templateDatas.ActiveTemplates[i], angleStart, angleExtent, 0.5, 1, 0.5, "true", 0,
        //                out HTuple row, out HTuple column, out HTuple angle, out HTuple score);

        //            activeIdentifyData.WheelTypes.Add(templateDatas.ActiveTemplateNames[i]);
        //            rows.Add(row);
        //            columns.Add(column);
        //            angles.Add(angle);

        //            if (score < 0.55)
        //                activeIdentifyData.Similaritys.Add(0.0);
        //            else
        //            {
        //                activeIdentifyData.Similaritys.Add(Math.Round(score.D, 3));
        //            }
        //        }
        //        //获取活跃模板匹配中的相似度最大值
        //        activeIdentifyData.Similarity = activeIdentifyData.Similaritys.Max();
        //    }
        //    else
        //        activeIdentifyData.Similarity = 0.0;

        //    //如果活跃模板匹配相似度最大值大于等于（系统设定识别成功的最小相似度 + 0.05 ），认为匹配成功 
        //    if (activeIdentifyData.Similarity >= minSimilarity + 0.05)
        //    {
        //        var index = activeIdentifyData.Similaritys.FindIndex(x => x == activeIdentifyData.Similarity);
        //        activeIdentifyData.RecognitionWheelType = activeIdentifyData.WheelTypes[index];
        //        activeIdentifyData.CenterRow = rows[index];
        //        activeIdentifyData.CenterColumn = columns[index];
        //        activeIdentifyData.Radian = angles[index];
        //        activeIdentifyData.TemplateID = templateDatas.ActiveTemplates[index];
        //        activeIdentifyData.IsInNotTemplate = false;
        //        return activeIdentifyData;
        //    }
        //    else
        //    {
        //        if (templateDatas.NotActiveTemplates.Count > 0)
        //        {
        //            for (int i = 0; i < templateDatas.NotActiveTemplates.Count; i++)
        //            {
        //                HOperatorSet.FindNccModel(image, templateDatas.NotActiveTemplates[i], angleStart, angleExtent, 0.5, 1, 0.5, "true", 0,
        //                    out HTuple row, out HTuple column, out HTuple angle, out HTuple score);

        //                notActiveIdentifyData.WheelTypes.Add(templateDatas.NotActiveTemplateNames[i]);
        //                activeIdentifyData.WheelTypes.Add(templateDatas.NotActiveTemplateNames[i]);
        //                rows.Add(row);
        //                columns.Add(column);
        //                angles.Add(angle);

        //                rowsN.Add(row);
        //                columnsN.Add(column);
        //                anglesN.Add(angle);

        //                if (score < 0.55)
        //                {
        //                    notActiveIdentifyData.Similaritys.Add(0.0);
        //                    activeIdentifyData.Similaritys.Add(0.0);
        //                }
        //                else
        //                {
        //                    notActiveIdentifyData.Similaritys.Add(Math.Round(score.D, 3));
        //                    activeIdentifyData.Similaritys.Add(Math.Round(score.D, 3));
        //                }
        //            }
        //            //获取不活跃模板匹配中的相似度最大值
        //            notActiveIdentifyData.Similarity = notActiveIdentifyData.Similaritys.Max();
        //            //如果活跃模板相似度最大值 大于等于 不活跃模板相似度最大值，且活跃模板相似度最大值 大于等于 设定值，则在活跃模板中识别成功
        //            if (activeIdentifyData.Similarity >= notActiveIdentifyData.Similarity && activeIdentifyData.Similarity >= minSimilarity)
        //            {
        //                var index = activeIdentifyData.Similaritys.FindIndex(x => x == activeIdentifyData.Similarity);
        //                activeIdentifyData.RecognitionWheelType = activeIdentifyData.WheelTypes[index];
        //                activeIdentifyData.CenterRow = rows[index];
        //                activeIdentifyData.CenterColumn = columns[index];
        //                activeIdentifyData.Radian = angles[index];
        //                activeIdentifyData.TemplateID = templateDatas.ActiveTemplates[index];
        //                activeIdentifyData.IsInNotTemplate = false;
        //                return activeIdentifyData;
        //            }
        //            //如果活跃模板相似度最大值 小于 不活跃模板相似度最大值，且不活跃模板相似度最大值 大于等于 设定值，则在不活跃模板中识别成功
        //            else if (activeIdentifyData.Similarity < notActiveIdentifyData.Similarity && notActiveIdentifyData.Similarity >= minSimilarity)
        //            {
        //                var index = notActiveIdentifyData.Similaritys.FindIndex(x => x == notActiveIdentifyData.Similarity);
        //                activeIdentifyData.RecognitionWheelType = notActiveIdentifyData.WheelTypes[index];
        //                activeIdentifyData.Similarity = notActiveIdentifyData.Similarity;
        //                activeIdentifyData.CenterRow = rowsN[index];
        //                activeIdentifyData.CenterColumn = columnsN[index];
        //                activeIdentifyData.Radian = anglesN[index];
        //                activeIdentifyData.TemplateID = templateDatas.NotActiveTemplates[index];
        //                activeIdentifyData.IsInNotTemplate = true;
        //                return activeIdentifyData;
        //            }
        //            //识别不成功
        //            else
        //            {
        //                activeIdentifyData.RecognitionWheelType = "NG";
        //                activeIdentifyData.Similarity = 0;
        //                activeIdentifyData.CenterRow = 0;
        //                activeIdentifyData.CenterColumn = 0;
        //                activeIdentifyData.Radian = 0;
        //                activeIdentifyData.TemplateID = null;
        //                activeIdentifyData.IsInNotTemplate = false;
        //                return activeIdentifyData;
        //            }
        //        }
        //        else
        //        {
        //            if (activeIdentifyData.Similarity >= minSimilarity)
        //            {
        //                var index = activeIdentifyData.Similaritys.FindIndex(x => x == activeIdentifyData.Similarity);
        //                activeIdentifyData.RecognitionWheelType = activeIdentifyData.WheelTypes[index];
        //                activeIdentifyData.CenterRow = rows[index];
        //                activeIdentifyData.CenterColumn = columns[index];
        //                activeIdentifyData.Radian = angles[index];
        //                activeIdentifyData.TemplateID = templateDatas.ActiveTemplates[index];
        //                activeIdentifyData.IsInNotTemplate = false;
        //                return activeIdentifyData;
        //            }
        //            else
        //            {
        //                activeIdentifyData.RecognitionWheelType = "NG";
        //                activeIdentifyData.Similarity = 0;
        //                activeIdentifyData.CenterRow = 0;
        //                activeIdentifyData.CenterColumn = 0;
        //                activeIdentifyData.Radian = 0;
        //                activeIdentifyData.TemplateID = null;
        //                activeIdentifyData.IsInNotTemplate = false;
        //                return activeIdentifyData;
        //            }
        //        }
        //    }
        //}

        public static RecognitionResultModel WheelRecognitionAlgorithm(HObject imageSource, List<TemplatedataModels> templateDatas, double angleStart,
                                                                        double angleExtent, double minSimilarity, List<RecognitionResultModel> recognitionResults)
        {
            HObject image = CloneImageSafely(imageSource);
            var resultIfFailed = new RecognitionResultModel
            {
                RecognitionWheelType = "NG"
            };

            foreach (var templateData in templateDatas.Where(t => t.Use == true))
            {
                if (templateData.Template != null)
                {
                    HOperatorSet.FindNccModel(
                    image, templateData.Template,
                    angleStart, angleExtent,
                    0.5, 1, 0.5,
                    "true", 0,
                    out HTuple row, out HTuple column, out HTuple angle, out HTuple score);

                    if (score != null && score.Length > 0 && score.D > 0.6)
                    {

                        recognitionResults.Add(new RecognitionResultModel
                        {
                            CenterRow = row.D,
                            CenterColumn = column.D,
                            Radian = angle.D,
                            RecognitionWheelType = templateData.WheelType,
                            Similarity = Math.Round(score.D, 3),
                            WheelStyle = templateData.WheelStyle
                        });
                    }
                    SafeHalconDispose(row);
                    SafeHalconDispose(column);
                    SafeHalconDispose(angle);
                    SafeHalconDispose(score);
                }


            }


            // 如果成功，尝试找最高相似度且满足阈值的
            if (TryGetBestMatch(recognitionResults, minSimilarity + 0.05, out var bestMatch))
            {
                bestMatch.status = "识别成功";
                //识别成功后 把这个轮形上次使用时间刷新
                string _type = bestMatch.RecognitionWheelType;
                var results = templateDatas
                    .Where(t => t.WheelType != null &&
                                t.WheelType == _type)
                    .ToList();

                // 输出结果
                foreach (var item in results)
                {
                    item.LastUsedTime = DateTime.Now;
                }
                return bestMatch;
            }

            // 活跃模板未匹配到，尝试非活跃模板
            //performMatching(false);
            foreach (var templateData in templateDatas.Where(t => t.Use == false))
            {
                if (templateData.Template != null)
                {

                    HOperatorSet.FindNccModel(image, templateData.Template,angleStart, angleExtent,
                            0.5, 1, 0.5,"true", 0,out HTuple row, out HTuple column, out HTuple angle, 
                            out HTuple score);

                    if (score != null && score.Length > 0 && score.D > 0.6)
                    {

                        recognitionResults.Add(new RecognitionResultModel
                        {
                            CenterRow = row.D,
                            CenterColumn = column.D,
                            Radian = angle.D,
                            RecognitionWheelType = templateData.WheelType,
                            Similarity = Math.Round(score.D, 3),
                            WheelStyle = templateData.WheelStyle
                        });
                    }
                    SafeHalconDispose(row);
                    SafeHalconDispose(column);
                    SafeHalconDispose(angle);
                    SafeHalconDispose(score);


                }


            }


            if (TryGetBestMatch(recognitionResults, minSimilarity + 0.05, out bestMatch))
            {
                bestMatch.status = "识别成功";
                //识别成功后 把这个轮形上次使用时间刷新
                string _type = bestMatch.RecognitionWheelType;
                var results = templateDatas
                    .Where(t => t.WheelType != null &&
                                t.WheelType == _type)
                    .ToList();

                // 输出结果
                foreach (var item in results)
                {
                    item.LastUsedTime = DateTime.Now;
                }
                return bestMatch;
            }
            SafeHalconDispose(image);
            return resultIfFailed;
        }

        // 辅助方法：从识别结果中找出最高相似度的对象
        private static bool TryGetBestMatch(
            List<RecognitionResultModel> results,
            double similarityThreshold,
            out RecognitionResultModel bestMatch)
        {
            if (results.Count == 0)
            {
                bestMatch = null;
                return false;
            }

            bestMatch = results.OrderByDescending(r => r.Similarity).First();

            return bestMatch.Similarity > similarityThreshold;
        }

        //public static RecognitionResultModel WheelRecognitionAlgorithm(HObject image, List<TemplatedataModels> templateDatas, double angleStart, double angleExtent, double minSimilarity, List<RecognitionResultModel> list = null)
        //{
        //    List<RecognitionResultModel> Recognition = new List<RecognitionResultModel>();
        //    RecognitionResultModel IdentifyData = new RecognitionResultModel();
        //    IdentifyData.status = "识别失败";
        //    IdentifyData.RecognitionWheelType = "NG";
        //    //活跃模板匹配，并将结果放入对应的匹配结果列表
        //    foreach (TemplatedataModels templateData in templateDatas)
        //    {
        //        if (templateData.Use)
        //        {
        //            HOperatorSet.FindNccModel(image, templateData.Template, angleStart, angleExtent, 0.5, 1, 0.5, "true", 0,
        //               out HTuple row, out HTuple column, out HTuple angle, out HTuple score);
        //            //查找记录保存起来 - 需要更新最后匹配时间
        //            if (score != null && score > 0.5)
        //            {
        //                RecognitionResultModel item = new RecognitionResultModel();
        //                item.CenterRow = row;
        //                item.CenterColumn = column;
        //                item.Radian = angle;
        //                item.RecognitionWheelType = templateData.TemplateName;
        //                item.Similarity = Math.Round(score.D, 3);
        //                Recognition.Add(item);
        //            }
        //        }
        //    }
        //    //活跃模板匹配完成
        //    if (Recognition.Count > 0)
        //    {
        //        double maxSim = Recognition.Max(x => x.Similarity);
        //        if (maxSim > minSimilarity + 0.05) //找到最大匹配相似度
        //        {
        //            IdentifyData = Recognition.Find(a => a.Similarity > maxSim);
        //            return IdentifyData;
        //        }
        //    }


        //    //活跃模板中未匹配到所需要的轮形
        //    foreach (TemplatedataModels templateData in templateDatas)
        //    {
        //        if (!templateData.Use)
        //        {

        //            HOperatorSet.FindNccModel(image, templateData.Template, angleStart, angleExtent, 0.5, 1, 0.5, "true", 0,
        //               out HTuple row, out HTuple column, out HTuple angle, out HTuple score);
        //            //查找记录保存起来 - 需要更新最后匹配时间
        //            if (score != null && score > 0.5)
        //            {
        //                RecognitionResultModel item = new RecognitionResultModel();
        //                item.CenterRow = row;
        //                item.CenterColumn = column;
        //                item.Radian = angle;
        //                item.RecognitionWheelType = templateData.TemplateName;
        //                item.Similarity = Math.Round(score.D, 3);
        //                Recognition.Add(item);
        //            }

        //        }
        //    }

        //    if (Recognition.Count > 0)
        //    {
        //        double maxSim = Recognition.Max(x => x.Similarity);
        //        if (maxSim > minSimilarity + 0.05) //找到最大匹配相似度
        //        {
        //            IdentifyData = Recognition.Find(a => a.Similarity > maxSim);
        //            return IdentifyData;
        //        }
        //    }
        //    return IdentifyData;



        //}


        public static HTuple WheelDeepLearning(HObject ho_ImageBatch, HTuple hv_DLModelHandle, HTuple hv_DLPreprocessParam)
        {
            HTuple hv_DLDeviceHandles = new HTuple(), hv_DLDevice = new HTuple();
            HTuple hv_ImageDir = new HTuple();
            HTuple hv_BatchSizeInference = new HTuple();
            HTuple hv_ClassNames = new HTuple();
            HTuple hv_ClassIDs = new HTuple();
            HTuple hv_WindowHandleDict = new HTuple(), hv_DLDataInfo = new HTuple();
            HTuple hv_GenParam = new HTuple(), hv_ImageFiles = new HTuple();
            HTuple hv_BatchIndex = new HTuple(), hv_Batch = new HTuple();
            HTuple hv_DLSampleBatch = new HTuple(), hv_DLResultBatch = new HTuple();
            HTuple hv_SampleIndex = new HTuple(), hv_DLSample = new HTuple();
            HTuple hv_DLResult = new HTuple(), hv_WindowHandles = new HTuple();
            //HOperatorSet.GenEmptyObj(out ho_ImageBatch);
            try
            {
                //找CPU或者GPU
                HOperatorSet.QueryAvailableDlDevices((new HTuple("runtime")).TupleConcat("runtime"),
                    (new HTuple("gpu")).TupleConcat("cpu"), out hv_DLDeviceHandles);
                if ((int)(new HTuple((new HTuple(hv_DLDeviceHandles.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    throw new HalconException("No supported device found to continue this example.");
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_DLDevice = hv_DLDeviceHandles.TupleSelect(
                        0);
                }


                //hv_ImageDir.Dispose();
                //hv_ImageDir = "D:/ZS/终检/DLT/picture/JG12345";
                //
                //大模型路径跟预参路径
                //hv_PreprocessParamFileName = "D:/ZS/终检/DLT/model_preprocess_params.hdict";
                //hv_RetrainedModelFileName = "D:/ZS/终检/DLT/model_opt.hdl";
                hv_BatchSizeInference = 1;
                //HOperatorSet.ReadDlModel(hv_RetrainedModelFileName, out hv_DLModelHandle);
                HOperatorSet.SetDlModelParam(hv_DLModelHandle, "batch_size", hv_BatchSizeInference);

                HOperatorSet.SetDlModelParam(hv_DLModelHandle, "device", hv_DLDevice);
                HOperatorSet.GetDlModelParam(hv_DLModelHandle, "class_names", out hv_ClassNames);
                HOperatorSet.GetDlModelParam(hv_DLModelHandle, "class_ids", out hv_ClassIDs);
                //HOperatorSet.ReadDict(hv_PreprocessParamFileName, new HTuple(), new HTuple(),
                //    out hv_DLPreprocessParam);

                HOperatorSet.CreateDict(out hv_WindowHandleDict);
                HOperatorSet.CreateDict(out hv_DLDataInfo);
                HOperatorSet.SetDictTuple(hv_DLDataInfo, "class_names", hv_ClassNames);
                HOperatorSet.SetDictTuple(hv_DLDataInfo, "class_ids", hv_ClassIDs);
                HOperatorSet.CreateDict(out hv_GenParam);
                HOperatorSet.SetDictTuple(hv_GenParam, "scale_windows", 1.1);

                HDevelopExport hDevelop = new HDevelopExport();


                hv_DLSampleBatch.Dispose();
                hDevelop.gen_dl_samples_from_images(ho_ImageBatch, out hv_DLSampleBatch);
                hDevelop.preprocess_dl_samples(hv_DLSampleBatch, hv_DLPreprocessParam);
                //
                HOperatorSet.ApplyDlModel(hv_DLModelHandle, hv_DLSampleBatch, new HTuple(),
                    out hv_DLResultBatch);
                //
                Console.WriteLine(hv_DLResultBatch.Length);
                if (hv_DLResultBatch.Length > 0)
                {
                    hv_SampleIndex = 0;
                    //
                    hv_DLSample.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_DLSample = hv_DLSampleBatch.TupleSelect(
                            hv_SampleIndex);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_DLResult = hv_DLResultBatch.TupleSelect(
                            hv_SampleIndex);
                    }
                    HOperatorSet.GetDictTuple(hv_DLResult, "classification_class_names", out HTuple names);
                    HOperatorSet.GetDictTuple(hv_DLResult, "classification_confidences", out HTuple confidences);
                    //for (int i = 0; i < names.Length; i++)
                    //{
                    //    Console.WriteLine($"数据：{names[i].S} 结果：{confidences[i].D.ToString("0.0000")}");
                    //}


                    return hv_DLResult;

                }
                else
                {
                    return null;
                }


                //
                //dev_display_dl_data(hv_DLSample, hv_DLResult, hv_DLDataInfo, "classification_result",
                //    hv_GenParam, hv_WindowHandleDict);
                //hv_WindowHandles.Dispose();
                //HOperatorSet.GetDictTuple(hv_WindowHandleDict, "classification_result",
                //    out hv_WindowHandles);
                //HDevWindowStack.SetActive(hv_WindowHandles.TupleSelect(
                //    0));
                //using (HDevDisposeHelper dh = new HDevDisposeHelper())
                //{
                //    set_display_font(hv_WindowHandles.TupleSelect(0), 16, "mono", "true", "false");
                //}
                //if (HDevWindowStack.IsOpen())
                //{
                //    HOperatorSet.DispText(HDevWindowStack.GetActive(), "Press Run (F5) to continue",
                //        "window", "bottom", "right", "black", new HTuple(), new HTuple());
                //}
                // stop(...); only in hdevelop


                //
                //Close windows used for visualization.
                // dev_close_window_dict(hv_WindowHandleDict);

            }
            catch (HalconException HDevExpDefaultException)
            {



                throw HDevExpDefaultException;

            }
            finally
            {

                hv_DLDeviceHandles.Dispose();
                hv_DLDevice.Dispose();
                //hv_ImageDir.Dispose();
                //hv_PreprocessParamFileName.Dispose();
                //hv_RetrainedModelFileName.Dispose();
                hv_BatchSizeInference.Dispose();
                //hv_DLModelHandle.Dispose();
                hv_ClassNames.Dispose();
                hv_ClassIDs.Dispose();
                //hv_DLPreprocessParam.Dispose();
                hv_WindowHandleDict.Dispose();
                hv_DLDataInfo.Dispose();
                hv_GenParam.Dispose();
                hv_ImageFiles.Dispose();
                hv_BatchIndex.Dispose();
                hv_Batch.Dispose();
                hv_DLSampleBatch.Dispose();
                hv_DLResultBatch.Dispose();
                hv_SampleIndex.Dispose();
                hv_DLSample.Dispose();

                hv_WindowHandles.Dispose();
            }




        }

        /// <summary>
        /// 获取仿射后的模板轮廓
        /// </summary>
        /// <param name="modelID">模板ID</param>
        /// <param name="newRow">新位置的行坐标</param>
        /// <param name="newColumn">新位置的列坐标</param>
        /// <param name="newRadian">新位置的角度</param>
        /// <param name="templateContour">仿射后的模板轮廓</param>
        public static HObject GetAffineTemplateContour(HTuple modelID, HTuple newRow, HTuple newColumn, HTuple newRadian)
        {
            HTuple model = modelID.Clone();
            HOperatorSet.GetNccModelRegion(out HObject modelRegion, model);
            HOperatorSet.VectorAngleToRigid(0, 0, 0, newRow, newColumn, newRadian, out HTuple homMat2D);
            HOperatorSet.AffineTransRegion(modelRegion, out HObject regionAffineTrans, homMat2D, "nearest_neighbor");
            HOperatorSet.GenContourRegionXld(regionAffineTrans, out HObject templateContour, "border_holes");

            SafeHalconDispose(modelRegion);
            SafeHalconDispose(model);
            SafeHalconDispose(newRow);
            SafeHalconDispose(newColumn);
            SafeHalconDispose(newRadian);
            SafeHalconDispose(homMat2D);
            SafeHalconDispose(regionAffineTrans);

            return templateContour;
        }

        /// <summary>
        /// 浇口检测算法
        /// </summary>
        /// <param name="image">源图像</param>
        /// <param name="centerRow">轮毂中心行坐标</param>
        /// <param name="centerColumn">轮毂中心列坐标</param>
        /// <param name="radius">图像中定位浇口区域的最小半径</param>
        /// <param name="gateMinThreshold">浇口区域最小阈值</param>
        /// <param name="gateMinArea">判断浇口存在的最小面积</param>
        /// <param name="gateMinRadius">判断浇口存在的最小半径</param>
        /// <param name="gateContour">最终找到的浇口轮廓</param>
        /// <returns>True为找到浇口</returns>
        public static GateDetectionResultModel GateDetection(HObject image, HTuple centerRow, HTuple centerColumn, HTuple radius, HTuple gateMinThreshold, HTuple gateMinArea, HTuple gateMinRadius)
        {
            GateDetectionResultModel result = new GateDetectionResultModel();
            //获取存在浇口区域
            HOperatorSet.GenCircle(out HObject circle, centerRow, centerColumn, radius);
            HOperatorSet.ReduceDomain(image, circle, out HObject imageReduced);
            //获取存在浇口区域面积
            HOperatorSet.AreaCenter(imageReduced, out HTuple area, out HTuple row, out HTuple column);
            //获取除浇口之外的轮毂区域
            HOperatorSet.Threshold(imageReduced, out HObject gateRegion, gateMinThreshold, 255);
            HOperatorSet.ClosingCircle(gateRegion, out HObject gateRegionClosing, 5.5);
            HOperatorSet.AreaCenter(gateRegionClosing, out HTuple gateArea, out HTuple row1, out HTuple column1);
            //计算存在浇口区域与浇口之外的轮毂区域的比例
            var areaProportion = gateArea.D / area.D;
            int addThreshold = 0;
            HOperatorSet.Intensity(imageReduced, image, out HTuple mean, out HTuple deviation);
            //如果比例大于0.7，说明浇口区域亮度存在问题，需要调整定位浇口之外的轮毂区域的阈值，再次判断
            if (areaProportion > 0.7 && deviation.D > 15)
            {
                Console.WriteLine($"亮度占比：{areaProportion} 偏差值：{deviation.D} 平均灰度值：{mean.D}");
                HOperatorSet.Threshold(imageReduced, out HObject newGateRegion, mean, 255);
                addThreshold = (int)(mean.D - gateMinThreshold.D);
                HOperatorSet.ClosingCircle(newGateRegion, out gateRegionClosing, 5.5);
                HOperatorSet.AreaCenter(gateRegionClosing, out HTuple newGateArea, out HTuple row2, out HTuple column2);
            }
            HOperatorSet.Difference(imageReduced, gateRegionClosing, out HObject regionDifference);
            HOperatorSet.ReduceDomain(imageReduced, regionDifference, out HObject endGateImage);
            HOperatorSet.Threshold(endGateImage, out HObject endGateRegion, 0, gateMinThreshold + addThreshold);
            HOperatorSet.Connection(endGateRegion, out HObject connectedRegions);
            HOperatorSet.SelectShapeStd(connectedRegions, out HObject selectedRegions, "max_area", 70);
            HOperatorSet.ClosingCircle(selectedRegions, out HObject regionClosing, 3.5);
            //获取最终浇口面积
            HOperatorSet.AreaCenter(regionClosing, out HTuple endArea, out HTuple row3, out HTuple column3);
            //获取最终浇口半径
            HOperatorSet.InnerCircle(regionClosing, out HTuple row4, out HTuple column4, out HTuple endRadius);
            HOperatorSet.GenCircleContourXld(out HObject contCircle, row4, column4, endRadius, 0, (new HTuple(360)).TupleRad(), "positive", 1.0);
            result.GateContour = contCircle;
            result.GateArea = endArea;
            result.GateRadiu = endRadius;
            if (endArea > gateMinArea && endRadius > gateMinRadius) result.DetectionResult = true;
            else result.DetectionResult = false;
            return result;
        }

        /// <summary>
        /// 获取线性渐变画刷
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static LinearGradientBrush GetLinearGradientBrush(Color color)//Colors.red
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0, 0);
            linearGradientBrush.EndPoint = new Point(0, 1);
            linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.0));
            linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1.2));
            return linearGradientBrush;
        }
    }
}
