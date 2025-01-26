using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgOpenGPS
{


    public class CRemoteControl
    {
        private readonly CModuleComm cModule;
        private readonly FormGPS mf;
        public CRemoteControl(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public void AblineSkipLeft()
        {
                mf.trk.NudgeTrack(-Properties.Settings.Default.setAS_snapDistance * 0.01);          
        }
       
        
        public void AblineSkipRight()
        {
           mf.trk.NudgeTrack(Properties.Settings.Default.setAS_snapDistance * 0.01);
        }

      




    }
}
