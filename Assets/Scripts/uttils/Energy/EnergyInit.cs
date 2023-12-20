using UnityEngine;

namespace uttils.Energy
{
    public class EnergyInit : DddD
    {
        public void Initialize()
        {
            UniWebView.SetAllowInlinePlay(true);

            var qqqYep = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (var popIt in qqqYep)
            {
                popIt.Stop();
            }

            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }
    }
}