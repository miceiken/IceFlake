using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatherPath.Graph
{
    public class SearchProgress
    {
        int searchID = 0, stepsProcessed = 0, prevStepsProcessed = 0;
        float distance;
        DateTime startTime = DateTime.Now, progressTime = DateTime.Now, lastSpam = DateTime.Now, pre = DateTime.Now;
        Spot src, dst;
        //bool isInCombat = GPlayerSelf.Me.IsInCombat;

        public SearchProgress(Spot src, Spot dst, int searchID)
        {
            this.searchID = searchID;
            this.src = src;
            this.dst = dst;
            distance = src.GetDistanceTo2D(dst);
        }

        public string Elapsed()
        {
            return (progressTime - startTime).ToString().Substring(0, 8);
        }
        public bool CheckProgress(Spot currentSearchSpot)
        {
            progressTime = System.DateTime.Now;
            if (stepsProcessed % 100 == 0)
            {
                System.TimeSpan span = System.DateTime.Now.Subtract(lastSpam);
                if (span.Seconds != 0)
                {
                    float distanceToDestination = currentSearchSpot.GetDistanceTo2D(dst);
                    var simplespot = new Location((float)Math.Round(currentSearchSpot.X), (float)Math.Round(currentSearchSpot.Y), (float)Math.Round(currentSearchSpot.Z));

                    //float completePercentage = (distance - distanceToDestination)*100 / distance;
                    //float bestPercentage = (distance - closestDistance)*100 / distance;

                    LogStatus(string.Format("{0}% on {1}, ", Math.Round((distance - distanceToDestination) * 100 / distance, 2), simplespot));

                    lastSpam = System.DateTime.Now;
                    prevStepsProcessed = stepsProcessed;
                }
            }
            stepsProcessed++;
            return true;
        }
        public void LogStatus(string result)
        {
            LogStatus(result, stepsProcessed);
        }
        public void LogStatus(string result, long cnt)
        {
            System.TimeSpan ts = System.DateTime.Now.Subtract(pre);
            int t = ts.Seconds * 1000 + ts.Milliseconds;
            if (t == 0) { t = 1; }
            Log(result + (cnt * 1000) / t + " steps/s");
        }
        private void Log(String s)
        {
            PatherPath.Logger.Log(s);
        }
    }
}
