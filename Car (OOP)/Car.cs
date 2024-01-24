using System;
using System.Collections.Generic;
using System.Text;

namespace Car__OOP_
{
    class Car
    {
        private int _speed;
        private int _wheels;
        private bool _engineOn;
        private bool _signalLightsOn;
        private int _fuel;
        private bool _breaksOn;

        public Car()
        {
            _speed = 0;
            _wheels = 4;
            _engineOn = false;
            _signalLightsOn = false;
            _fuel = 30;
            _breaksOn = true;
        }

    }

}
