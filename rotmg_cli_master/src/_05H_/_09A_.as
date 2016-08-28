// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_05H_._09A_

package _05H_{
    import _E_7._0J_n;

    public class _09A_ extends _U_y {

        private var itemXML:XML;
        private var curItemXML:XML;
        private var _E_y:XML;
        private var _0F_j:XML;

        override protected function compareSlots(_arg1:XML, _arg2:XML):void{
            var _local3:String;
            this.itemXML = _arg1;
            this.curItemXML = _arg2;
            _local3 = "";
            _t4 = "";
            if (_arg1.hasOwnProperty("NumProjectiles"))
            {
                _local3 = this._go();
                _t4 = (_t4 + _local3);
                _5n[_arg1.NumProjectiles.toXMLString()] = _local3;
            };
            if (_arg1.hasOwnProperty("Projectile"))
            {
                _local3 = this._56();
                _t4 = (_t4 + _local3);
                _5n[_arg1.Projectile.toXMLString()] = _local3;
            };
            this._R_J_();
        }
        private function _56():String{
            var _local1:String = this._wD_();
            var _local2:Number = ((Number(this._E_y.Speed) * Number(this._E_y.LifetimeMS)) / 10000);
            var _local3:Number = ((Number(this._0F_j.Speed) * Number(this._0F_j.LifetimeMS)) / 10000);
            var _local4:String = _0J_n._A_l(_local2);
            _local1 = (_local1 + (_qF_("Range: ", _u8) + _qF_((_local4 + "\n"), _qy((_local2 - _local3)))));
            if (this._E_y.hasOwnProperty("MultiHit"))
            {
                _local1 = (_local1 + _qF_("Shots hit multiple targets\n", _iF_));
            };
            if (this._E_y.hasOwnProperty("PassesCover"))
            {
                _local1 = (_local1 + _qF_("Shots pass through obstacles\n", _iF_));
            };
            return (_local1);
        }
        private function _go():String{
            var _local1:int = int(this.itemXML.NumProjectiles);
            var _local2:int = int(this.curItemXML.NumProjectiles);
            var _local3:String = _qy((_local1 - _local2));
            return (((_qF_("Shots: ", _u8) + _qF_(_local1.toString(), _local3)) + "\n"));
        }
        private function _wD_():String{
            var _local1 = "";
            this._E_y = XML(this.itemXML.Projectile);
            var _local2:int = int(this._E_y.MinDamage);
            var _local3:int = int(this._E_y.MaxDamage);
            var _local4:Number = ((_local3 + _local2) / 2);
            this._0F_j = XML(this.curItemXML.Projectile);
            var _local5:int = int(this._0F_j.MinDamage);
            var _local6:int = int(this._0F_j.MaxDamage);
            var _local7:Number = ((_local6 + _local5) / 2);
            var _local8:String = (((_local2 == _local3)) ? _local2 : ((_local2 + " - ") + _local3)).toString();
            return (((_qF_("Damage: ", _u8) + _qF_(_local8, _qy((_local4 - _local7)))) + "\n"));
        }
        private function _R_J_():void{
            if ((((this.itemXML.RateOfFire.length() == 0)) || ((this.curItemXML.RateOfFire.length() == 0))))
            {
                return;
            };
            var _local1:Number = Number(this.curItemXML.RateOfFire[0]);
            var _local2:Number = Number(this.itemXML.RateOfFire[0]);
            var _local3:int = int(((_local2 / _local1) * 100));
            var _local4:int = (_local3 - 100);
            if (_local4 == 0)
            {
                return;
            };
            var _local5:String = _qy(_local4);
            var _local6:String = _local4.toString();
            if (_local4 > 0)
            {
                _local6 = ("+" + _local6);
            };
            _local6 = _qF_((_local6 + "%"), _local5);
            _t4 = (_t4 + (("Rate of Fire: " + _local6) + "\n"));
            _5n[this.itemXML.RateOfFire[0].toXMLString()];
        }

    }
}//package _05H_

