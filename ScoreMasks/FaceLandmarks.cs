namespace StringArt.ScoreMasks;
using static StringArt.ScoreMasks.FacePart;

public static class FaceLandmarks
{
    public static Dictionary<FacePart, IReadOnlyList<uint>> FacePartPoints = new()
    {
        { Jawline, new List<uint>{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16} },
        { RightEyebrow, new List<uint>{17,18,19,20,21} },
        { LeftEyebrow, new List<uint>{22,23,24,25,26} },
        { NoseBridge, new List<uint>{27,28,29,30} },
        { NoseTip, new List<uint>{31,32,33,34,35} },
        { RightEye, new List<uint>{36,37,38,39,40,41} },
        { LeftEye, new List<uint>{42,43,44,45,46,47} },
        { LipsOuterEdge, new List<uint>{48,49,50,51,52,53,54,55,56,57,58,59,60} },
        { LipsInnerEdge, new List<uint>{61,62,63,64,65,66,67} }
    };
}
