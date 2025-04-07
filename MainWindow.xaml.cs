using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Mineamator_to_Blockbench_Converter
{
    public static class AnimationHelper
    {

        private static List<KeyValuePair<double, double[]>> Ease(double[] startValues, double[] endValues, double startTime, double endTime, int steps, Func<double, double> easingFunction)
        {
            if (startValues.Length != endValues.Length)
            {
                throw new ArgumentException("Start and end value arrays must have the same length.");
            }

            List<KeyValuePair<double, double[]>> intermediateFrames = new List<KeyValuePair<double, double[]>>();
            double duration = endTime - startTime;
            int dimensions = startValues.Length;

            for (int i = 1; i < steps; i++)
            {
                double t = i / (double)steps;
                double easedT = easingFunction(t);
                double[] currentValues = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    currentValues[j] = startValues[j] + (endValues[j] - startValues[j]) * easedT;
                }
                double currentTime = startTime + duration * t;
                intermediateFrames.Add(new KeyValuePair<double, double[]>(currentTime, currentValues));
            }
            return intermediateFrames;
        }

        // Bezier
        public static List<KeyValuePair<double, double[]>> EaseQuadraticBezier(double[] p0, double[] p1, double[] p2, double startTime, double endTime, int steps)
        {
            if (p0.Length != p1.Length || p0.Length != p2.Length)
            {
                throw new ArgumentException("Control point arrays must have the same length.");
            }

            List<KeyValuePair<double, double[]>> intermediateFrames = new List<KeyValuePair<double, double[]>>();
            double duration = endTime - startTime;
            int dimensions = p0.Length;

            for (int i = 1; i < steps; i++)
            {
                double t = i / (double)steps;
                double[] currentValues = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    currentValues[j] = (1 - t) * (1 - t) * p0[j] + 2 * (1 - t) * t * p1[j] + t * t * p2[j];
                }
                double currentTime = startTime + duration * t;
                intermediateFrames.Add(new KeyValuePair<double, double[]>(currentTime, currentValues));
            }
            return intermediateFrames;
        }

        public static List<KeyValuePair<double, double[]>> EaseCubicBezier(double[] p0, double[] p1, double[] p2, double[] p3, double startTime, double endTime, int steps)
        {
            if (p0.Length != p1.Length || p0.Length != p2.Length || p0.Length != p3.Length)
            {
                throw new ArgumentException("Control point arrays must have the same length.");
            }

            List<KeyValuePair<double, double[]>> intermediateFrames = new List<KeyValuePair<double, double[]>>();
            double duration = endTime - startTime;
            int dimensions = p0.Length;

            for (int i = 1; i < steps; i++)
            {
                double t = i / (double)steps;
                double[] currentValues = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    currentValues[j] = Math.Pow(1 - t, 3) * p0[j] + 3 * Math.Pow(1 - t, 2) * t * p1[j] + 3 * (1 - t) * Math.Pow(t, 2) * p2[j] + Math.Pow(t, 3) * p3[j];
                }
                double currentTime = startTime + duration * t;
                intermediateFrames.Add(new KeyValuePair<double, double[]>(currentTime, currentValues));
            }
            return intermediateFrames;
        }

        // Quadratic
        public static List<KeyValuePair<double, double[]>> EaseInQuad(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t * t);
        public static List<KeyValuePair<double, double[]>> EaseOutQuad(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => 1 - (1 - t) * (1 - t));
        public static List<KeyValuePair<double, double[]>> EaseInOutQuad(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t < 0.5 ? 2 * t * t : 1 - Math.Pow(-2 * t + 2, 2) / 2);

        // Cubic
        public static List<KeyValuePair<double, double[]>> EaseInCubic(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t * t * t);
        public static List<KeyValuePair<double, double[]>> EaseOutCubic(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => 1 - Math.Pow(1 - t, 3));
        public static List<KeyValuePair<double, double[]>> EaseInOutCubic(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t < 0.5 ? 4 * t * t * t : 1 - Math.Pow(-2 * t + 2, 3) / 2);

        // Quartic
        public static List<KeyValuePair<double, double[]>> EaseInQuart(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t * t * t * t);
        public static List<KeyValuePair<double, double[]>> EaseOutQuart(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => 1 - Math.Pow(1 - t, 4));
        public static List<KeyValuePair<double, double[]>> EaseInOutQuart(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t < 0.5 ? 8 * t * t * t * t : 1 - Math.Pow(-2 * t + 2, 4) / 2);

        // Quintic
        public static List<KeyValuePair<double, double[]>> EaseInQuint(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t * t * t * t * t);
        public static List<KeyValuePair<double, double[]>> EaseOutQuint(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => 1 + Math.Pow(t - 1, 5));
        public static List<KeyValuePair<double, double[]>> EaseInOutQuint(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t < 0.5 ? 16 * t * t * t * t * t : 1 + 16 * Math.Pow(t - 0.5, 5) / 2);

        // Sine
        public static List<KeyValuePair<double, double[]>> EaseInSine(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => 1 - Math.Cos(t * Math.PI / 2));
        public static List<KeyValuePair<double, double[]>> EaseOutSine(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => Math.Sin(t * Math.PI / 2));
        public static List<KeyValuePair<double, double[]>> EaseInOutSine(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => (1 - Math.Cos(Math.PI * t)) / 2);

        // Exponential
        public static List<KeyValuePair<double, double[]>> EaseInExpo(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t == 0 ? 0 : Math.Pow(2, 10 * (t - 1)));
        public static List<KeyValuePair<double, double[]>> EaseOutExpo(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t == 1 ? 1 : 1 - Math.Pow(2, -10 * t));
        public static List<KeyValuePair<double, double[]>> EaseInOutExpo(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t == 0 ? 0 : t == 1 ? 1 : t < 0.5 ? Math.Pow(2, 20 * t - 10) / 2 : (2 - Math.Pow(2, -20 * t + 10)) / 2);

        // Circular
        public static List<KeyValuePair<double, double[]>> EaseInCirc(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => 1 - Math.Sqrt(1 - Math.Pow(t, 2)));
        public static List<KeyValuePair<double, double[]>> EaseOutCirc(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => Math.Sqrt(1 - Math.Pow(t - 1, 2)));
        public static List<KeyValuePair<double, double[]>> EaseInOutCirc(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t < 0.5 ? (1 - Math.Sqrt(1 - Math.Pow(2 * t, 2))) / 2 : (Math.Sqrt(1 - Math.Pow(-2 * t + 2, 2)) + 1) / 2);

        // Elastic
        public static List<KeyValuePair<double, double[]>> EaseInElastic(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => { const double c4 = (2 * Math.PI) / 3; return t == 0 ? 0 : t == 1 ? 1 : -Math.Pow(2, 10 * t - 10) * Math.Sin((t * 10 - 10.75) * c4); });
        public static List<KeyValuePair<double, double[]>> EaseOutElastic(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => { const double c4 = (2 * Math.PI) / 3; return t == 0 ? 0 : t == 1 ? 1 : Math.Pow(2, -10 * t) * Math.Sin((t * 10 - 0.75) * c4) + 1; });
        public static List<KeyValuePair<double, double[]>> EaseInOutElastic(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => { const double c5 = (2 * Math.PI) / 4.5; return t == 0 ? 0 : t == 1 ? 1 : t < 0.5 ? -(Math.Pow(2, 20 * t - 10) * Math.Sin((20 * t - 11.125) * c5)) / 2 : (Math.Pow(2, -20 * t + 10) * Math.Sin((20 * t - 11.125) * c5)) / 2 + 1; });

        // Back
        public static List<KeyValuePair<double, double[]>> EaseInBack(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => { const double c1 = 1.70158; return c1 * t * t * t - c1 * t * t; });
        public static List<KeyValuePair<double, double[]>> EaseOutBack(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => { const double c1 = 1.70158; return 1 + c1 * Math.Pow(t - 1, 3) + c1 * Math.Pow(t - 1, 2); });
        public static List<KeyValuePair<double, double[]>> EaseInOutBack(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => { const double c1 = 1.70158; const double c2 = c1 * 1.525; return t < 0.5 ? (Math.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2 : (Math.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2; });

        // Bounce
        public static List<KeyValuePair<double, double[]>> EaseInBounce(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => 1 - EaseOutBounceInternal(1 - t));
        public static List<KeyValuePair<double, double[]>> EaseOutBounce(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, EaseOutBounceInternal);
        public static List<KeyValuePair<double, double[]>> EaseInOutBounce(double[] startValues, double[] endValues, double startTime, double endTime, int steps) => Ease(startValues, endValues, startTime, endTime, steps, t => t < 0.5 ? (1 - EaseOutBounceInternal(1 - 2 * t)) / 2 : (1 + EaseOutBounceInternal(2 * t - 1)) / 2);

        private static double EaseOutBounceInternal(double n)
        {
            const double n1 = 7.5625;
            const double d1 = 2.75;

            if (n < 1 / d1)
            {
                return n1 * n * n;
            }
            else if (n < 2 / d1)
            {
                return n1 * (n -= 1.5 / d1) * n + 0.75;
            }
            else if (n < 2.5 / d1)
            {
                return n1 * (n -= 2.25 / d1) * n + 0.9375;
            }
            else
            {
                return n1 * (n -= 2.625 / d1) * n + 0.984375;
            }
        }
    }
    public class PrePostValue
    {
        public double[]? pre { get; set; }
        public double[]? post { get; set; }
    }

    // Temporary class to hold transition type during parsing
    public class IntermediateKeyframeData
    {
        public double[]? Value { get; set; }
        public string? TransitionType { get; set; }
    }

    public class BoneData
    {
        public Dictionary<string, object>? position { get; set; }
        public Dictionary<string, object>? rotation { get; set; }
        public Dictionary<string, object>? scale { get; set; }
    }

    public class OutputAnimation
    {
        public string loop { get; set; } = "hold_on_last_frame";
        public double animation_length { get; set; }
        public Dictionary<string, BoneData> bones { get; set; } = new Dictionary<string, BoneData>();
    }

    public class OutputAnimations
    {
        public OutputAnimation animation { get; set; }
    }

    public class OutputFormat
    {
        public string format_version { get; set; } = "1.8.0";
        public OutputAnimations animations { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 




    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public  void ConvertJson(string text, string fileName, string fullpath)
        {
            try
            {
                JsonDocument parsed = JsonDocument.Parse(text);
                JsonElement root = parsed.RootElement;

                if (!root.TryGetProperty("tempo", out JsonElement tempoElement) || tempoElement.ValueKind != JsonValueKind.Number)
                {
                    Console.WriteLine($"Error: 'tempo' not found or is not a number in {fileName}");
                    return;
                }
                long tempo = tempoElement.GetInt64();

                if (!root.TryGetProperty("length", out JsonElement lengthElement) || lengthElement.ValueKind != JsonValueKind.Number)
                {
                    Console.WriteLine($"Error: 'length' not found or is not a number in {fileName}");
                    return;
                }
                long length = lengthElement.GetInt64();


                string? rootBoneNameInput = ThingN.Text;

                var bonesIntermediate = new Dictionary<string, BoneData>();

                if (root.TryGetProperty("keyframes", out JsonElement keyframesElement) && keyframesElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement keyframe in keyframesElement.EnumerateArray())
                    {
                        if (!keyframe.TryGetProperty("position", out JsonElement positionElement) || positionElement.ValueKind != JsonValueKind.Number)
                        {
                            Console.WriteLine($"Warning: 'position' not found or is not a number in a keyframe of {fileName}");
                            continue;
                        }
                        long position = positionElement.GetInt64();

                        string? boneName = null;
                        if (keyframe.TryGetProperty("part_name", out JsonElement partNameElement) && partNameElement.ValueKind == JsonValueKind.String)
                        {
                            boneName = partNameElement.GetString();
                        }
                        else
                        {
                            boneName = rootBoneNameInput;
                        }

                        if (string.IsNullOrEmpty(boneName))
                        {
                            Console.WriteLine($"Warning: Could not determine bone name for a keyframe in {fileName}");
                            continue;
                        }

                        double xPos = 0, yPos = 0, zPos = 0;
                        double xRot = 0, yRot = 0, zRot = 0;
                        double xSca = 1, ySca = 1, zSca = 1;
                        string transitiontype = null;

                        if (keyframe.TryGetProperty("values", out JsonElement valuesElement) && valuesElement.ValueKind == JsonValueKind.Object)
                        {
                            if (valuesElement.TryGetProperty("POS_X", out JsonElement posXElement))
                            {
                                xPos = posXElement.ValueKind == JsonValueKind.Number ? posXElement.GetDouble() : 0;
                            }
                            if (valuesElement.TryGetProperty("POS_Y", out JsonElement posYElement))
                            {
                                yPos = posYElement.ValueKind == JsonValueKind.Number ? posYElement.GetDouble() : 0;
                            }
                            if (valuesElement.TryGetProperty("POS_Z", out JsonElement posZElement))
                            {
                                zPos = posZElement.ValueKind == JsonValueKind.Number ? posZElement.GetDouble() : 0;
                            }
                            if (valuesElement.TryGetProperty("ROT_X", out JsonElement rotXElement))
                            {
                                xRot = rotXElement.ValueKind == JsonValueKind.Number ? rotXElement.GetDouble() : 0;
                            }
                            if (valuesElement.TryGetProperty("ROT_Y", out JsonElement rotYElement))
                            {
                                yRot = rotYElement.ValueKind == JsonValueKind.Number ? rotYElement.GetDouble() : 0;
                            }
                            if (valuesElement.TryGetProperty("ROT_Z", out JsonElement rotZElement))
                            {
                                zRot = rotZElement.ValueKind == JsonValueKind.Number ? rotZElement.GetDouble() : 0;
                            }
                            if (valuesElement.TryGetProperty("SCA_X", out JsonElement scaXElement))
                            {
                                xSca = scaXElement.ValueKind == JsonValueKind.Number ? scaXElement.GetDouble() : 1;
                            }
                            if (valuesElement.TryGetProperty("SCA_Y", out JsonElement scaYElement))
                            {
                                ySca = scaYElement.ValueKind == JsonValueKind.Number ? scaYElement.GetDouble() : 1;
                            }
                            if (valuesElement.TryGetProperty("SCA_Z", out JsonElement scaZElement))
                            {
                                zSca = scaZElement.ValueKind == JsonValueKind.Number ? scaZElement.GetDouble() : 1;
                            }
                            if (valuesElement.TryGetProperty("TRANSITION", out JsonElement transitiontypeElement))
                            {
                                transitiontype = transitiontypeElement.GetString();
                            }
                        }

                        double[] posData = { xPos, zPos, -yPos };
                        double[] rotData = { xRot, -zRot, -yRot };
                        double[] scaleData = { xSca, zSca, ySca };

                        string framePosString = (position / (double)tempo).ToString();

                        if (!bonesIntermediate.ContainsKey(boneName))
                        {
                            bonesIntermediate[boneName] = new BoneData
                            {
                                position = new Dictionary<string, object>(),
                                rotation = new Dictionary<string, object>(),
                                scale = new Dictionary<string, object>()
                            };
                        }

                        bonesIntermediate[boneName].position[framePosString] = posData;
                        bonesIntermediate[boneName].rotation[framePosString] = rotData;
                        bonesIntermediate[boneName].scale[framePosString] = scaleData;
                    }
                }

                var finalBones = new Dictionary<string, BoneData>();

                foreach (var boneEntry in bonesIntermediate)
                {
                    string currentBoneName = boneEntry.Key;
                    finalBones[currentBoneName] = new BoneData
                    {
                        position = new Dictionary<string, object>(),
                        rotation = new Dictionary<string, object>(),
                        scale = new Dictionary<string, object>()
                    };

                    foreach (var propertyName in new[] { "position", "rotation", "scale" })
                    {
                        var keyframes = boneEntry.Value.GetType().GetProperty(propertyName)?.GetValue(boneEntry.Value) as Dictionary<string, object>;
                        var finalKeyframes = finalBones[currentBoneName].GetType().GetProperty(propertyName)?.GetValue(finalBones[currentBoneName]) as Dictionary<string, object>;

                        if (keyframes != null && keyframes.Count > 1)
                        {
                            var sortedKeys = keyframes.Keys.OrderBy(double.Parse).ToList();

                            for (int i = 0; i < sortedKeys.Count - 1; i++)
                            {
                                var currentTimeStr = sortedKeys[i];
                                var nextTimeStr = sortedKeys[i + 1];
                                double currentTime = double.Parse(currentTimeStr);
                                double nextTime = double.Parse(nextTimeStr);
                                double[] startValue = keyframes[currentTimeStr] as double[];
                                double[] endValue = keyframes[nextTimeStr] as double[];

                                finalKeyframes[currentTimeStr] = startValue; // Add the original keyframe

                                // Retrieve transition type (this requires re-parsing or storing it)
                                string transitionType = null;
                                if (root.TryGetProperty("keyframes", out JsonElement keyframesElementx) && keyframesElementx.ValueKind == JsonValueKind.Array)
                                {
                                    foreach (JsonElement keyframe in keyframesElementx.EnumerateArray())
                                    {
                                        if (keyframe.TryGetProperty("position", out JsonElement posElement) && posElement.ValueKind == JsonValueKind.Number && (long)(posElement.GetInt64() / (double)tempo * tempo) == (long)(currentTime * tempo))
                                        {
                                            if (keyframe.TryGetProperty("values", out JsonElement valuesElement) && valuesElement.ValueKind == JsonValueKind.Object)
                                            {
                                                if (valuesElement.TryGetProperty("TRANSITION", out JsonElement transitiontypeElement))
                                                {
                                                    transitionType = transitiontypeElement.GetString();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(transitionType))
                                {
                                    transitionType = transitionType.ToLower();
                                    int steps = 10;
                                    List<KeyValuePair<double, double[]>>? intermediateFrames = null;

                                    if (transitionType == "instant")
                                    {
                                        // Handle instant transition by setting post value of current to next
                                        if (finalKeyframes.ContainsKey(currentTimeStr) && endValue != null)
                                        {
                                            finalKeyframes[currentTimeStr] = new Dictionary<string, double[]> { { "post", endValue } };
                                        }
                                    }
                                    else if (transitionType == "easeinquad" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInQuad(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutquad" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutQuad(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutquad" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutQuad(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeincubic" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInCubic(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutcubic" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutCubic(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutcubic" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutCubic(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinquart" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInQuart(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutquart" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutQuart(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutquart" && startValue != null && endValue != null) // Already handled above, but kept for clarity
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutQuart(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinquint" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInQuint(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutquint" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutQuint(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutquint" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutQuint(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutsine" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutSine(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinsine" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInSine(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutsine" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutSine(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinexpo" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInExpo(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutexpo" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutExpo(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutexpo" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutExpo(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeincirc" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInCirc(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutcirc" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutCirc(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutcirc" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutCirc(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinelastic" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInElastic(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutelastic" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutElastic(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutelastic" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutElastic(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinback" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInBack(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutback" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutBack(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutback" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutBack(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinbounce" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInBounce(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeoutbounce" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseOutBounce(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "easeinoutbounce" && startValue != null && endValue != null)
                                    {
                                        intermediateFrames = AnimationHelper.EaseInOutBounce(startValue, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "bezierquadratic" && startValue != null && endValue != null)
                                    {
                                        // Placeholder for extracting control point 1 (p1)
                                        double[] p1 = startValue.Select(v => v + (endValue[0] - startValue[0]) / 3).ToArray(); // Example
                                        intermediateFrames = AnimationHelper.EaseQuadraticBezier(startValue, p1, endValue, currentTime, nextTime, steps);
                                    }
                                    else if (transitionType == "beziercubic" && startValue != null && endValue != null)
                                    {
                                        // Placeholder for extracting control points 1 (p1) and 2 (p2)
                                        double[] p1 = startValue.Select(v => v + (endValue[0] - startValue[0]) / 3).ToArray(); // Example
                                        double[] p2 = endValue.Select(v => v - (endValue[0] - startValue[0]) / 3).ToArray(); // Example
                                        intermediateFrames = AnimationHelper.EaseCubicBezier(startValue, p1, p2, endValue, currentTime, nextTime, steps);
                                    }
                                    // Add other curve types here

                                    if (intermediateFrames != null)
                                    {
                                        for (int j = 0; j < intermediateFrames.Count; j++)
                                        {
                                            double intermediateTime = intermediateFrames[j].Key;
                                            double[] intermediateValue = intermediateFrames[j].Value;

                                            double previousTime = (j > 0) ? intermediateFrames[j - 1].Key : currentTime;
                                            double[] previousValue = (j > 0) ? intermediateFrames[j - 1].Value : startValue;

                                            var prePostData = new Dictionary<string, object>();
                                            prePostData["pre"] = previousValue; // Assuming previousValue is a double[]
                                            prePostData["post"] = intermediateValue; // Assuming intermediateValue is a double[]
                                            prePostData["lerp_mode"] = "bezier";

                                            finalKeyframes[intermediateTime.ToString("G17")] = prePostData;
                                        }
                                    }
                                }
                            }
                            if (sortedKeys.Any())
                            {
                                finalKeyframes[sortedKeys.Last()] = keyframes[sortedKeys.Last()];
                            }
                        }
                        else if (keyframes != null && keyframes.Count == 1)
                        {
                            foreach (var kvp in keyframes)
                            {
                                finalKeyframes[kvp.Key] = kvp.Value;
                            }
                        }
                    }
                }

                var output = new OutputFormat
                {
                    animations = new OutputAnimations
                    {
                        animation = new OutputAnimation
                        {
                            animation_length = (double)length / tempo,
                            bones = finalBones
                        }
                    }
                };

                string outputFileName =  Path.ChangeExtension(fullpath, ".animation.json");
                Console.WriteLine( outputFileName );
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(output, options);

                // Convert scientific notation to decimal
                string rebuiltText = Regex.Replace(jsonString, @"(\d+\.?\d*)e([+-]?\d+)", match =>
                {
                    if (double.TryParse(match.Value, out double value))
                    {
                        return value.ToString("G17"); // Use "G17" for maximum precision
                    }
                    return match.Value;
                });

                File.WriteAllText(outputFileName, rebuiltText);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON in {fileName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing {fileName}: {ex.Message}");
            }
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);


                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        if (Path.GetFileName(file).EndsWith(".miframes")) ;
                        string filename = Path.GetFileNameWithoutExtension(file);



                        string jsonfile = File.ReadAllText(file);
                        ConvertJson(jsonfile, filename,Path.GetFullPath(file));
                    }
                }
            }
        }
    }
}