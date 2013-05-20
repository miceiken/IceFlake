namespace IceFlake.Client.API
{
    public abstract class Frame
    {
        public Frame(string frameName)
        {
            Name = frameName;
        }

        public string Name { get; private set; }

        public bool Exists
        {
            get { return WoWScript.Execute<bool>(Name + " ~= nil"); }
        }

        public bool IsShown
        {
            get { return Exists && WoWScript.Execute<bool>(Name + ":IsShown()"); }
        }

        public void Hide()
        {
            Hide(Name);
        }

        public static void Hide(string frameName)
        {
            WoWScript.ExecuteNoResults(frameName + ":Hide()");
        }

        //public static explicit operator Frame(string frameName)
        //{
        //    return new Frame(frameName);
        //}
    }
}