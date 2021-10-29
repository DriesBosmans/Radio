using MagicGradients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MOB_RadioApp.Services
{
    public class AnimationService
    {
        public static async void AnimateBackground(MagicGradients.GradientView view, uint time = 45000)
        {
            
            int delay = (int)time;
            Action<double> forward = input => view.AnchorY = input;
            Action<double> backward = input => view.AnchorY = input;
            while (true)
            {
                view.Animate(name: "forward", callback: forward, start: 0, end: 1, length: time, easing: Easing.SinInOut);
                await Task.Delay(delay);
                view.Animate(name: "backward", callback: backward, start: 1, end: 0, length: time, easing: Easing.SinInOut);
                await Task.Delay(delay);
            }
        }
    }
}
