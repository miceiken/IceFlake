namespace PatherPath.Graph
{
    public interface ILocationHeuristics
    {
        float Score(float x, float y, float z);
    }
}