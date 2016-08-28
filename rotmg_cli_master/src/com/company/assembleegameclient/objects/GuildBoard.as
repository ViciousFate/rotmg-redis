// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.objects.GuildBoard

package com.company.assembleegameclient.objects{
    import _R_v._S_p;
    import com.company.assembleegameclient.game.GameSprite;
    import _R_v.Panel;

    public class GuildBoard extends GameObject implements _G_4 {

        public function GuildBoard(_arg1:XML){
            super(_arg1);
            _064 = true;
        }
        public function _C_s(_arg1:GameSprite):Panel{
            return (new _S_p(_arg1));
        }

    }
}//package com.company.assembleegameclient.objects

