// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_vf._gs

package _vf{
    import flash.media.Sound;
    import flash.media.SoundChannel;
    import flash.net.URLRequest;
    import com.company.assembleegameclient.parameters.Parameters;
    import flash.media.SoundTransform;

    public class _gs {

        private static var _059:Sound = null;
        private static var _Z_z:SoundChannel = null;

        public static function load():void{
            _059 = new Sound();
            _059.load(new URLRequest((("http://" + Parameters.musicUrl_) + "/sfx/music/main.mp3")));
            _Z_z = _059.play(0, int.MAX_VALUE, new SoundTransform(((Parameters.data_.playMusic) ? 0.65 : 0)));
        }
        public static function _continue(_arg1:Boolean):void{
            Parameters.data_.playMusic = _arg1;
            Parameters.save();
            _Z_z.soundTransform = new SoundTransform(((Parameters.data_.playMusic) ? 0.65 : 0));
        }

    }
}//package _vf

