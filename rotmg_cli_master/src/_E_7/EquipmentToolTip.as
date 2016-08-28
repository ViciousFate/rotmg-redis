// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_E_7.EquipmentToolTip

package _E_7{
    import flash.display.Bitmap;
    import com.company.ui.SimpleText;
    import com.company.assembleegameclient.ui._return;
    import com.company.assembleegameclient.objects.Player;

    import com.company.assembleegameclient.objects.ObjectLibrary;
    import flash.display.BitmapData;
    import com.company.util.BitmapUtil;
    import flash.filters.DropShadowFilter;
    import com.company.assembleegameclient.net.messages.data.StatData;
    import _ke._U_c;
    import com.company.assembleegameclient.parameters.Parameters;
    import com.company.util._H_V_;
    import flash.text.StyleSheet;


    public class EquipmentToolTip extends _for_ {

        private static const _be:int = 230;
        private static const _K_z:String = ".in { margin-left:10px; text-indent: -10px; }";

        private var _5U_:Bitmap;
        private var _P_V_:SimpleText;
        private var _V_0:SimpleText;
        private var _C_G_:SimpleText;
        private var line1_:_return;
        private var _f8:SimpleText;
        private var line2_:_return;
        private var _tF_:SimpleText;
        private var player_:Player;
        private var _Y_X_:Boolean = false;
        private var objectType_:int;
        private var _3z:XML = null;
        private var _I_z:XML = null;
        private var _uW_:_fM_;
        private var _0G_J_:Boolean;
        private var _Q_O_:Vector.<Restriction>;
        private var _a2:Vector.<Effect>;
        private var _0A_f:int;
        private var _sJ_:int;
        private var _J_9:String;
        private var _G_H_:uint;
        private var _bs:Boolean;
        private var _02N_:int;
        private var _gE_:_R_N_;

        public function EquipmentToolTip(_arg1:int, _arg2:Player, _arg3:int, _arg4:String, _arg5:uint=1, _arg6:Boolean=false){
            var _local9:uint;
            this.player_ = _arg2;
            this._J_9 = _arg4;
            this._G_H_ = _arg5;
            this._bs = _arg6;
            this._0G_J_ = (((_arg2)!=null) ? ObjectLibrary._d1(_arg1, _arg2) : false);
            var _local7:uint = ((((this._0G_J_) || ((this.player_ == null)))) ? 0x363636 : 6036765);
            var _local8:uint = ((((this._0G_J_) || ((_arg2 == null)))) ? 0x9B9B9B : 10965039);
            super(_local7, 1, _local8, 1, true);
            this._uW_ = new _fM_();
            this.objectType_ = _arg1;
            this._I_z = ObjectLibrary._Q_F_[_arg1];
            this._Y_X_ = (((this.player_)!=null) ? ObjectLibrary._01j(this.objectType_, this.player_) : false);
            this._a2 = new Vector.<Effect>();
            this._sJ_ = _arg3;
            this._0A_f = int(this._I_z.SlotType);
            if (this.player_ == null)
            {
                this._3z = this._I_z;
            } else
            {
                if (this._Y_X_)
                {
                    _local9 = 0;
                    while (_local9 < 4)
                    {
                        if ((((this._0A_f == this.player_._9A_[_local9])) && (!((this.player_._zq[_local9] == -1)))))
                        {
                            this._3z = ObjectLibrary._Q_F_[this.player_._zq[_local9]];
                            break;
                        };
                        _local9++;
                    };
                };
            };
            this._0B_C_();
            this._0D_6();
            this._07X_();
            this._ev();
            this._case_();
            this._U_I_();
            this._eS_();
            this._R_H_();
            this._jY_();
            this._S_F_();
            this._K_6();
            this._ff();
            this._i5();
            this._02u();
            this._V_Y_();
        }
        private static function _N_W_(_arg1:Vector.<Restriction>):String{
            var _local4:Restriction;
            var _local5:String;
            var _local2 = "";
            var _local3:Boolean = true;
            for each (_local4 in _arg1)
            {
                if (!_local3)
                {
                    _local2 = (_local2 + "\n");
                } else
                {
                    _local3 = false;
                };
                _local5 = (((('<font color="#' + _local4.color_.toString(16)) + '">') + _local4.text_) + "</font>");
                if (_local4.bold_)
                {
                    _local5 = (("<b>" + _local5) + "</b>");
                };
                _local2 = (_local2 + _local5);
            };
            return (_local2);
        }

        private function _02c():Boolean{
            return (((this._Y_X_) && ((this._3z == null))));
        }
        private function _0B_C_():void{
            var _local1:XML = ObjectLibrary._Q_F_[this.objectType_];
            var _local2:int = 5;
            if (_local1.hasOwnProperty("ScaleValue"))
            {
                _local2 = _local1.ScaleValue;
            };
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(this.objectType_, 60, true, true, _local2);
            _local3 = BitmapUtil._Y_d(_local3, 4, 4, (_local3.width - 8), (_local3.height - 8));
            this._5U_ = new Bitmap(_local3);
            addChild(this._5U_);
        }
        private function _07X_():void{
            this._V_0 = new SimpleText(16, 0xFFFFFF, false, 30, 0, "Myriad Pro");
            this._P_V_.setBold(true);
            this._V_0.y = ((this._5U_.height / 2) - (this._P_V_._I_x / 2));
            this._V_0.x = (_be - 30);
            if ((((this._I_z.hasOwnProperty("Consumable") == false)) && ((this._tO_() == false))))
            {
                if (this._I_z.hasOwnProperty("Tier"))
                {
                    this._V_0.text = ("T" + this._I_z.Tier);
                } else
                {
                    this._V_0.setColor(9055202);
                    this._V_0.text = "UT";
                };
                this._V_0.updateMetrics();
                addChild(this._V_0);
            };
        }
        private function _tO_():Boolean{
            var activateTags:XMLList;
            activateTags = this._I_z.Activate.(text() == "PermaPet");
            return ((activateTags.length() >= 1));
        }
        private function _0D_6():void{
            var _local1:int = ((((this._0G_J_) || ((this.player_ == null)))) ? 0xFFFFFF : 16549442);
            this._P_V_ = new SimpleText(16, _local1, false, (((_be - this._5U_.width) - 4) - 30), 0, "Myriad Pro");
            this._P_V_.setBold(true);
            this._P_V_.wordWrap = true;
            this._P_V_.text = ObjectLibrary._0D_N_[this.objectType_];
            this._P_V_.updateMetrics();
            this._P_V_.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
            this._P_V_.x = (this._5U_.width + 4);
            this._P_V_.y = ((this._5U_.height / 2) - (this._P_V_._I_x / 2));
            addChild(this._P_V_);
        }
        private function _i5():void{
            this._02N_ = ((this._C_G_.y + this._C_G_.height) + 8);
            if (((!((this._a2.length == 0))) || (!((this._gE_.text == "")))))
            {
                this.line1_ = new _return((_be - 12), 0);
                this.line1_.x = 8;
                this.line1_.y = this._02N_;
                addChild(this.line1_);
                this._f8 = new SimpleText(14, 0xB3B3B3, false, ((_be - this._5U_.width) - 4), 0, "Myriad Pro");
                this._f8.wordWrap = true;
                this._f8.htmlText = (this._gE_.text + this._41(this._a2));
                this._f8._08S_();
                this._f8.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
                this._f8.x = 4;
                this._f8.y = (this.line1_.y + 8);
                addChild(this._f8);
                this._02N_ = ((this._f8.y + this._f8.height) + 8);
            };
        }
        private function _U_I_():void{
            if (((this._I_z.hasOwnProperty("NumProjectiles")) && (!((this._gE_._5n.hasOwnProperty(this._I_z.NumProjectiles.toXMLString()) == true)))))
            {
                this._a2.push(new Effect("Shots", this._I_z.NumProjectiles));
            };
        }
        private function _ff():void{
            var _local1:int;
            var _local2:String;
            var _local3:String;
            var _local4:int;
            if (this._I_z.hasOwnProperty("FameBonus"))
            {
                _local1 = int(this._I_z.FameBonus);
                _local2 = (_local1 + "%");
                _local3 = ((this._0G_J_) ? _0J_n._rJ_ : _0J_n._iF_);
                if (((!((this._3z == null))) && (this._3z.hasOwnProperty("FameBonus"))))
                {
                    _local4 = int(this._3z.FameBonus.text());
                    _local3 = _0J_n._qy((_local1 - _local4));
                };
                this._a2.push(new Effect("Fame Bonus", _0J_n._M_w(_local2, _local3)));
            };
        }
        private function _K_6():void{
            if (((this._I_z.hasOwnProperty("MpCost")) && (!(this._gE_._5n[this._I_z.MpCost[0].toXMLString()]))))
            {
                this._a2.push(new Effect("MP Cost", this._I_z.MpCost));
            };
        }
        private function _S_F_():void{
            if (this._I_z.hasOwnProperty("Doses"))
            {
                this._a2.push(new Effect("Doses", this._I_z.Doses));
            };
        }
        private function _eS_():void{
            var _local1:XML;
            var _local2:int;
            var _local3:int;
            var _local4:Number;
            var _local5:XML;
            if (((this._I_z.hasOwnProperty("Projectile")) && ((this._gE_._5n.hasOwnProperty(this._I_z.Projectile.toXMLString()) == false))))
            {
                _local1 = XML(this._I_z.Projectile);
                _local2 = int(_local1.MinDamage);
                _local3 = int(_local1.MaxDamage);
                this._a2.push(new Effect("Damage", (((_local2 == _local3)) ? _local2 : ((_local2 + " - ") + _local3)).toString()));
                _local4 = ((Number(_local1.Speed) * Number(_local1.LifetimeMS)) / 10000);
                this._a2.push(new Effect("Range", _0J_n._A_l(_local4)));
                if (this._I_z.Projectile.hasOwnProperty("MultiHit"))
                {
                    this._a2.push(new Effect("", "Shots hit multiple targets"));
                };
                if (this._I_z.Projectile.hasOwnProperty("PassesCover"))
                {
                    this._a2.push(new Effect("", "Shots pass through obstacles"));
                };
                for each (_local5 in _local1.ConditionEffect)
                {
                    if (this._gE_._5n[_local5.toXMLString()] == null)
                    {
                        this._a2.push(new Effect("Shot Effect", (((this._I_z.Projectile.ConditionEffect + " for ") + this._I_z.Projectile.ConditionEffect.@duration) + " secs")));
                    };
                };
            };
        }
        private function _R_H_():void{
            var _local1:XML;
            var _local2:String;
            var _local3:int;
            var _local4:int;
            for each (_local1 in this._I_z.Activate)
            {
                if (this._gE_._5n[_local1.toXMLString()] != true)
                {
                    if (_local1.toString() == "ConditionEffectAura")
                    {
                        this._a2.push(new Effect("Party Effect", (("Within " + _local1.@range) + " sqrs")));
                        this._a2.push(new Effect("", (((("  " + _local1.@effect) + " for ") + _local1.@duration) + " secs")));
                    } else
                    {
                        if (_local1.toString() == "ConditionEffectSelf")
                        {
                            this._a2.push(new Effect("Effect on Self", ""));
                            this._a2.push(new Effect("", (((("  " + _local1.@effect) + " for ") + _local1.@duration) + " secs")));
                        } else
                        {
                            if (_local1.toString() == "Heal")
                            {
                                this._a2.push(new Effect("", (("+" + _local1.@amount) + " HP")));
                            } else
                            {
                                if (_local1.toString() == "HealNova")
                                {
                                    this._a2.push(new Effect("Party Heal", (((_local1.@amount + " HP at ") + _local1.@range) + " sqrs")));
                                } else
                                {
                                    if (_local1.toString() == "Magic")
                                    {
                                        this._a2.push(new Effect("", (("+" + _local1.@amount) + " MP")));
                                    } else
                                    {
                                        if (_local1.toString() == "MagicNova")
                                        {
                                            this._a2.push(new Effect("Fill Party Magic", (((_local1.@amount + " MP at ") + _local1.@range) + " sqrs")));
                                        } else
                                        {
                                            if (_local1.toString() == "Teleport")
                                            {
                                                this._a2.push(new Effect("", "Teleport to Target"));
                                            } else
                                            {
                                                if (_local1.toString() == "VampireBlast")
                                                {
                                                    this._a2.push(new Effect("Steal", (((_local1.@totalDamage + " HP within ") + _local1.@radius) + " sqrs")));
                                                } else
                                                {
                                                    if (_local1.toString() == "Trap")
                                                    {
                                                        this._a2.push(new Effect("Trap", (((_local1.@totalDamage + " HP within ") + _local1.@radius) + " sqrs")));
                                                        this._a2.push(new Effect("", (((("  " + ((_local1.hasOwnProperty("@condEffect")) ? _local1.@condEffect : "Slowed")) + " for ") + ((_local1.hasOwnProperty("@condDuration")) ? _local1.@condDuration : "5")) + " secs")));
                                                    } else
                                                    {
                                                        if (_local1.toString() == "StasisBlast")
                                                        {
                                                            this._a2.push(new Effect("Stasis on Group", (_local1.@duration + " secs")));
                                                        } else
                                                        {
                                                            if (_local1.toString() == "Decoy")
                                                            {
                                                                this._a2.push(new Effect("Decoy", (_local1.@duration + " secs")));
                                                            } else
                                                            {
                                                                if (_local1.toString() == "Lightning")
                                                                {
                                                                    this._a2.push(new Effect("Lightning", ""));
                                                                    this._a2.push(new Effect("", ((((" " + _local1.@totalDamage) + " to ") + _local1.@maxTargets) + " targets")));
                                                                } else
                                                                {
                                                                    if (_local1.toString() == "PoisonGrenade")
                                                                    {
                                                                        this._a2.push(new Effect("Poison Grenade", ""));
                                                                        this._a2.push(new Effect("", ((((((" " + _local1.@totalDamage) + " HP over ") + _local1.@duration) + " secs within ") + _local1.@radius) + " sqrs\n")));
                                                                    } else
                                                                    {
                                                                        if (_local1.toString() == "RemoveNegativeConditions")
                                                                        {
                                                                            this._a2.push(new Effect("", "Removes negative conditions"));
                                                                        } else
                                                                        {
                                                                            if (_local1.toString() == "RemoveNegativeConditionsSelf")
                                                                            {
                                                                                this._a2.push(new Effect("", "Removes negative conditions"));
                                                                            } else
                                                                            {
                                                                                if (_local1.toString() == "IncrementStat")
                                                                                {
                                                                                    _local3 = int(_local1.@stat);
                                                                                    _local4 = int(_local1.@amount);
                                                                                    if (((!((_local3 == StatData._V_A_))) && (!((_local3 == StatData._aC_)))))
                                                                                    {
                                                                                        _local2 = ("Permanently increases " + StatData._W_H_(_local3));
                                                                                    } else
                                                                                    {
                                                                                        _local2 = ((("+" + _local4) + " ") + StatData._W_H_(_local3));
                                                                                    };
                                                                                    this._a2.push(new Effect("", _local2));
                                                                                };
                                                                            };
                                                                        };
                                                                    };
                                                                };
                                                            };
                                                        };
                                                    };
                                                };
                                            };
                                        };
                                    };
                                };
                            };
                        };
                    };
                };
            };
        }
        private function _jY_():void{
            var _local1:XML;
            var _local3:String;
            var _local2:Boolean = true;
            for each (_local1 in this._I_z.ActivateOnEquip)
            {
                if (_local2)
                {
                    this._a2.push(new Effect("On Equip", ""));
                    _local2 = false;
                };
                _local3 = this._gE_._P_3[_local1.toXMLString()];
                if (_local3 != null)
                {
                    this._a2.push(new Effect("", (" " + _local3)));
                } else
                {
                    if (_local1.toString() == "IncrementStat")
                    {
                        this._a2.push(new Effect("", this._gN_(_local1)));
                    };
                };
            };
        }
        private function _gN_(activateXML:XML):String{
            var match:XML;
            var otherAmount:int;
            var stat:int = int(activateXML.@stat);
            var amount:int = int(activateXML.@amount);
            var textColor:String = ((this._0G_J_) ? _0J_n._rJ_ : _0J_n._iF_);
            var otherMatches:XMLList;
            if (this._3z != null)
            {
                otherMatches = this._3z.ActivateOnEquip.(@stat == stat);
            };
            if (((!((otherMatches == null))) && ((otherMatches.length() == 1))))
            {
                match = XML(otherMatches[0]);
                otherAmount = int(match.@amount);
                textColor = _0J_n._qy((amount - otherAmount));
            };
            return (_0J_n._M_w(((("+" + amount) + " ") + StatData._W_H_(stat)), textColor));
        }
        private function _B_3():void{
            this._Q_O_.push(new Restriction("Must be equipped to use", 0xB3B3B3, false));
            if (((this._bs) || ((this._J_9 == _U_c.CURRENT_PLAYER))))
            {
                this._Q_O_.push(new Restriction("Double-Click to equip", 0xB3B3B3, false));
            } else
            {
                this._Q_O_.push(new Restriction("Double-Click to take", 0xB3B3B3, false));
            };
        }
        private function _V_X_():void{
            this._Q_O_.push(new Restriction((("Press [" + _H_V_._in[Parameters.data_.useSpecial]) + "] in world to use"), 0xFFFFFF, false));
        }
        private function _vK_():void{
            this._Q_O_.push(new Restriction("Consumed with use", 0xB3B3B3, false));
            if (((this._bs) || ((this._J_9 == _U_c.CURRENT_PLAYER))))
            {
                this._Q_O_.push(new Restriction("Double-Click or Shift-Click on item to use", 0xFFFFFF, false));
            } else
            {
                this._Q_O_.push(new Restriction("Double-Click to take & Shift-Click to use", 0xFFFFFF, false));
            };
        }
        private function _6C_():void{
            this._Q_O_.push(new Restriction("Can be used multiple times", 0xB3B3B3, false));
            this._Q_O_.push(new Restriction("Double-Click or Shift-Click on item to use", 0xFFFFFF, false));
        }
        private function _02u():void{
            var _local2:XML;
            var _local3:Boolean;
            var _local4:int;
            var _local5:int;
            this._Q_O_ = new Vector.<Restriction>();
            if (((((this._I_z.hasOwnProperty("VaultItem")) && (!((this._sJ_ == -1))))) && (!((this._sJ_ == ObjectLibrary._pb["Vault Chest"])))))
            {
                this._Q_O_.push(new Restriction("Store this item in your Vault to avoid losing it!", 16549442, true));
            };
            if (this._I_z.hasOwnProperty("Soulbound"))
            {
                this._Q_O_.push(new Restriction("Soulbound", 0xB3B3B3, false));
            };
            if (this._0G_J_)
            {
                if (this._I_z.hasOwnProperty("Usable"))
                {
                    this._V_X_();
                    this._B_3();
                } else
                {
                    if (this._I_z.hasOwnProperty("Consumable"))
                    {
                        this._vK_();
                    } else
                    {
                        if (this._I_z.hasOwnProperty("InvUse"))
                        {
                            this._6C_();
                        } else
                        {
                            this._B_3();
                        };
                    };
                };
            } else
            {
                if (this.player_ != null)
                {
                    this._Q_O_.push(new Restriction(("Not usable by " + ObjectLibrary._0D_N_[this.player_.objectType_]), 16549442, true));
                };
            };
            var _local1:Vector.<String> = ObjectLibrary._7S_(this.objectType_);
            if (_local1 != null)
            {
                this._Q_O_.push(new Restriction(("Usable by: " + _local1.join(", ")), 0xB3B3B3, false));
            };
            for each (_local2 in this._I_z.EquipRequirement)
            {
                _local3 = ObjectLibrary._get(_local2, this.player_);
                if (_local2.toString() == "Stat")
                {
                    _local4 = int(_local2.@stat);
                    _local5 = int(_local2.@value);
                    this._Q_O_.push(new Restriction(((("Requires " + StatData._W_H_(_local4)) + " of ") + _local5), ((_local3) ? 0xB3B3B3 : 16549442), ((_local3) ? false : true)));
                };
            };
        }
        private function _V_Y_():void{
            var _local1:StyleSheet;
            if (this._Q_O_.length != 0)
            {
                this.line2_ = new _return((_be - 12), 0);
                this.line2_.x = 8;
                this.line2_.y = this._02N_;
                addChild(this.line2_);
                _local1 = new StyleSheet();
                _local1.parseCSS(_K_z);
                this._tF_ = new SimpleText(14, 0xB3B3B3, false, (_be - 4), 0, "Myriad Pro");
                this._tF_.styleSheet = _local1;
                this._tF_.wordWrap = true;
                this._tF_.htmlText = (("<span class='in'>" + _N_W_(this._Q_O_)) + "</span>");
                this._tF_._08S_();
                this._tF_.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
                this._tF_.x = 4;
                this._tF_.y = (this.line2_.y + 8);
                addChild(this._tF_);
            };
        }
        private function _ev():void{
            this._C_G_ = new SimpleText(14, 0xB3B3B3, false, _be, 0, "Myriad Pro");
            this._C_G_.wordWrap = true;
            this._C_G_.text = String(this._I_z.Description);
            this._C_G_.updateMetrics();
            this._C_G_.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
            this._C_G_.x = 4;
            this._C_G_.y = (this._5U_.height + 2);
            addChild(this._C_G_);
        }
        private function _case_():void{
            if (this._3z != null)
            {
                this._gE_ = this._uW_._hS_(this._I_z, this._3z);
            } else
            {
                this._gE_ = new _R_N_();
            };
        }
        private function _41(_arg1:Vector.<Effect>):String{
            var _local4:Effect;
            var _local5:String;
            var _local2 = "";
            var _local3:Boolean = true;
            for each (_local4 in _arg1)
            {
                _local5 = "#FFFF8F";
                if (!_local3)
                {
                    _local2 = (_local2 + "\n");
                } else
                {
                    _local3 = false;
                };
                if (_local4.name_ != "")
                {
                    _local2 = (_local2 + (_local4.name_ + ": "));
                };
                if (this._02c())
                {
                    _local5 = "#00ff00";
                };
                _local2 = (_local2 + (((('<font color="' + _local5) + '">') + _local4.value_) + "</font>"));
            };
            return (_local2);
        }

    }
}//package _E_7

class Effect {

    public var name_:String;
    public var value_:String;

    public function Effect(_arg1:String, _arg2:String){
        this.name_ = _arg1;
        this.value_ = _arg2;
    }
}
class Restriction {

    public var text_:String;
    public var color_:uint;
    public var bold_:Boolean;

    public function Restriction(_arg1:String, _arg2:uint, _arg3:Boolean){
        this.text_ = _arg1;
        this.color_ = _arg2;
        this.bold_ = _arg3;
    }
}

