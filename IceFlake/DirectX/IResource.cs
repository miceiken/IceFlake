namespace IceFlake.DirectX
{
    public interface IResource
    {
        void OnLostDevice();
        void OnResetDevice();
        void Draw();
    }
}