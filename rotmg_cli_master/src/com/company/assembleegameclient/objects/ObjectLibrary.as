// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.objects.ObjectLibrary

package com.company.assembleegameclient.objects{
    import flash.utils.Dictionary;

    import _0_P_._0F_7;
    import flash.utils.getDefinitionByName;
    import flash.display.BitmapData;
    import com.company.util.AssetLibrary;
    import com.company.assembleegameclient.util.TextureRedrawer;
    import com.company.assembleegameclient.ui.Slot;
    import com.company.util.ConversionUtil;
    import com.company.assembleegameclient.net.messages.data.StatData;

	import com.company.assembleegameclient.objects.Wall;

    public class ObjectLibrary {

        public static const _020:Dictionary = new Dictionary();
        public static const _Q_F_:Dictionary = new Dictionary();
        public static const _pb:Dictionary = new Dictionary();
        public static const _0D_N_:Dictionary = new Dictionary();
        public static const _V_a:Dictionary = new Dictionary();
        public static const _5Q_:Dictionary = new Dictionary();
        public static const _Q_z:Dictionary = new Dictionary();
        public static const _w8:ObjectProperties = new ObjectProperties(null);
        public static const objLibrary:Object = {
            "CaveWall":CaveWall,
            "Character":Character,
            "CharacterChanger":CharacterChanger,
            "ClosedVaultChest":ClosedVaultChest,
            "ConnectedWall":ConnectedWall,
            "Container":Container,
            "GameObject":GameObject,
            "GuildBoard":GuildBoard,
            "GuildChronicle":GuildChronicle,
            "GuildHallPortal":GuildHallPortal,
            "GuildMerchant":GuildMerchant,
            "GuildRegister":GuildRegister,
            "Merchant":Merchant,
            "MoneyChanger":MoneyChanger,
            "NameChanger":NameChanger,
            "Player":Player,
            "Portal":Portal,
            "Projectile":Projectile,
            "Sign":Sign,
            "SpiderWeb":SpiderWeb,
            "Stalagmite":Stalagmite,
            "Wall":Wall
        }

        public static var _tj:Vector.<XML> = new Vector.<XML>();
        public static var _J_G_:Vector.<XML> = new Vector.<XML>();
        public static var _9x:Dictionary = new Dictionary();

        public static function _nY_(_arg1:XML):void{
            var _local2:XML;
            var _local3:String;
            var _local4:String;
            var _local5:int;
            var _local6:Boolean;
            var _local7:int;
            for each (_local2 in _arg1.Object)
            {
                _local3 = String(_local2.@id);
                _local4 = _local3;
                if (_local2.hasOwnProperty("DisplayId"))
                {
                    _local4 = _local2.DisplayId;
                };
                if (_local2.hasOwnProperty("Group"))
                {
                    if (_local2.Group == "Hexable")
                    {
                        _J_G_.push(_local2);
                    };
                };
                _local5 = int(_local2.@type);
                _020[_local5] = new ObjectProperties(_local2);
                _Q_F_[_local5] = _local2;
                _pb[_local3] = _local5;
                _0D_N_[_local5] = _local4;
                if (String(_local2.Class) == "Player")
                {
                    _9x[_local5] = String(_local2.@id).substr(0, 2);
                    _local6 = false;
                    _local7 = 0;
                    while (_local7 < _tj.length)
                    {
                        if (int(_tj[_local7].@type) == _local5)
                        {
                            _tj[_local7] = _local2;
                            _local6 = true;
                        };
                        _local7++;
                    };
                    if (!_local6)
                    {
                        _tj.push(_local2);
                    };
                };
                _V_a[_local5] = new _Z_H_(_local2);
                if (_local2.hasOwnProperty("Top"))
                {
                    _5Q_[_local5] = new _Z_H_(XML(_local2.Top));
                };
                if (_local2.hasOwnProperty("Animation"))
                {
                    _Q_z[_local5] = new _0F_7(_local2);
                };
            };
        }
        public static function _6H_(_arg1:int):String{
            var _local2:XML = _Q_F_[_arg1];
            if (_local2 == null)
            {
                return (null);
            };
            return (String(_local2.@id));
        }
        public static function _W_x(_arg1:String):ObjectProperties{
            var _local2:int = _pb[_arg1];
            return (_020[_local2]);
        }
        public static function _077(_arg1:String):XML{
            var _local2:int = _pb[_arg1];
            return (_Q_F_[_local2]);
        }
        public static function _075(_arg1:int):GameObject{
            var _local2:XML = _Q_F_[_arg1];
            var _local3:String = _local2.Class;
            var _local4:Class = ((objLibrary[_local3]) || (_076(_local3)));
            //var _local4:GameObject = new (_local3)(_local2);
            return (new (_local4)(_local2));
        }
        private static function _076(_arg1:String):Class {
            var _local2:String = ("com.company.assembleegameclient.objects." + _arg1);
            return ((getDefinitionByName(_local2) as Class));
        }
        public static function getTextureFromType(_arg1:int):BitmapData{
            var _local2:_Z_H_ = _V_a[_arg1];
            if (_local2 == null)
            {
                return (null);
            };
            return (_local2.getTexture());
        }
        public static function getRedrawnTextureFromType(_arg1:int, _arg2:int, _arg3:Boolean, _arg4:Boolean=true, _arg5:int=5):BitmapData{
            var _local6:_Z_H_ = _V_a[_arg1];
            var _local7:BitmapData = _local6.getTexture();
            if (_local7 == null)
            {
                _local7 = AssetLibrary._xK_("lofiObj3", 0xFF);
            };
            var _local8:BitmapData = _local6.mask_;
            if (_local8 == null)
            {
                return (TextureRedrawer.redraw(_local7, _arg2, _arg3, 0, 0, _arg4, _arg5));
            };
            var _local9:XML = _Q_F_[_arg1];
            var _local10:int = ((_local9.hasOwnProperty("Tex1")) ? int(_local9.Tex1) : 0);
            var _local11:int = ((_local9.hasOwnProperty("Tex2")) ? int(_local9.Tex2) : 0);
            _local7 = TextureRedrawer.resize(_local7, _local8, _arg2, _arg3, _local10, _local11);
            _local7 = TextureRedrawer.outlineGlow(_local7, 0, 0);
            return (_local7);
        }
        public static function _P_N_(_arg1:int):int{
            var _local2:XML = _Q_F_[_arg1];
            if (!_local2.hasOwnProperty("Size"))
            {
                return (100);
            };
            return (int(_local2.Size));
        }
        public static function _gf(_arg1:int):int{
            var _local2:XML = _Q_F_[_arg1];
            if (!_local2.hasOwnProperty("SlotType"))
            {
                return (-1);
            };
            return (int(_local2.SlotType));
        }
        public static function _01j(_arg1:int, _arg2:Player):Boolean{
            if (_arg1 == -1)
            {
                return (false);
            };
            var _local3:XML = _Q_F_[_arg1];
            var _local4:int = int(_local3.SlotType.toString());
            var _local5:uint;
            while (_local5 < 4)
            {
                if (_arg2._9A_[_local5] == _local4)
                {
                    return (true);
                };
                _local5++;
            };
            return (false);
        }
        public static function _d1(_arg1:int, _arg2:Player):Boolean{
            if (_arg2 == null)
            {
                return (true);
            };
            var _local3:XML = _Q_F_[_arg1];
            if ((((_local3 == null)) || (!(_local3.hasOwnProperty("SlotType")))))
            {
                return (false);
            };
            var _local4:int = _local3.SlotType;
            if (_local4 == Slot._U_l)
            {
                return (true);
            };
            var _local5:int;
            while (_local5 < _arg2._9A_.length)
            {
                if (_arg2._9A_[_local5] == _local4)
                {
                    return (true);
                };
                _local5++;
            };
            return (false);
        }
        public static function _0H_Z_(_arg1:int):Boolean{
            var _local2:XML = _Q_F_[_arg1];
            return (((!((_local2 == null))) && (_local2.hasOwnProperty("Soulbound"))));
        }
        public static function _7S_(_arg1:int):Vector.<String>{
            var _local5:XML;
            var _local6:Vector.<int>;
            var _local7:int;
            var _local2:XML = _Q_F_[_arg1];
            if ((((_local2 == null)) || (!(_local2.hasOwnProperty("SlotType")))))
            {
                return (null);
            };
            var _local3:int = _local2.SlotType;
            if ((((_local3 == Slot._U_l)) || ((_local3 == Slot._yW_))))
            {
                return (null);
            };
            var _local4:Vector.<String> = new Vector.<String>();
            for each (_local5 in _tj)
            {
                _local6 = ConversionUtil._04n(_local5.SlotTypes);
                _local7 = 0;
                while (_local7 < _local6.length)
                {
                    if (_local6[_local7] == _local3)
                    {
                        _local4.push(_0D_N_[int(_local5.@type)]);
                        break;
                    };
                    _local7++;
                };
            };
            return (_local4);
        }
        public static function _S_d(_arg1:int, _arg2:Player):Boolean{
            var _local4:XML;
            if (_arg2 == null)
            {
                return (true);
            };
            var _local3:XML = _Q_F_[_arg1];
            for each (_local4 in _local3.EquipRequirement)
            {
                if (!_get(_local4, _arg2))
                {
                    return (false);
                };
            };
            return (true);
        }
        public static function _get(_arg1:XML, _arg2:Player):Boolean{
            var _local3:int;
            if (_arg1.toString() == "Stat")
            {
                _local3 = int(_arg1.@value);
                switch (int(_arg1.@stat))
                {
                    case StatData._0I_1:
                        return ((_arg2._L_T_ >= _local3));
                    case StatData._079:
                        return ((_arg2._a7 >= _local3));
                    case StatData._70:
                        return ((_arg2._81 >= _local3));
                    case StatData._0R_:
                        return ((_arg2._hh >= _local3));
                    case StatData._sw:
                        return ((_arg2._ai >= _local3));
                    case StatData._00l:
                        return ((_arg2.speed_ >= _local3));
                    case StatData._S_2:
                        return ((_arg2._kO_ >= _local3));
                    case StatData._0G_F_:
                        return ((_arg2._0F_f >= _local3));
                    case StatData._0J_z:
                        return ((_arg2._gc >= _local3));
                };
            };
            return (false);
        }

    }
}//package com.company.assembleegameclient.objects

