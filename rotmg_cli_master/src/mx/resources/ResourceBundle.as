package mx.resources
{
   import mx.core.mx_internal;
   import flash.system.ApplicationDomain;
   
   use namespace mx_internal;
   
   public class ResourceBundle extends Object implements IResourceBundle
   {
      
      public function ResourceBundle(param1:String=null, param2:String=null) {
         this._content = {};
         super();
         this._locale = param1;
         this._bundleName = param2;
         this._content = this.getContent();
      }
      
      mx_internal  static const VERSION:String = "4.6.0.0";
      
      mx_internal  static var locale:String;
      
      mx_internal  static var backupApplicationDomain:ApplicationDomain;
      
      private static function getClassByName(param1:String, param2:ApplicationDomain) : Class {
         var _loc3_:Class = null;
         if(param2.hasDefinition(param1))
         {
            _loc3_ = param2.getDefinition(param1) as Class;
         }
         return _loc3_;
      }
      
      mx_internal var _bundleName:String;
      
      public function get bundleName() : String {
         return this._bundleName;
      }
      
      private var _content:Object;
      
      public function get content() : Object {
         return this._content;
      }
      
      mx_internal var _locale:String;
      
      public function get locale() : String {
         return this._locale;
      }
      
      protected function getContent() : Object {
         return {};
      }
      
      private function _getObject(param1:String) : Object {
         var _loc2_:Object = this.content[param1];
         if(!_loc2_)
         {
            throw new Error("Key " + param1 + " was not found in resource bundle " + this.bundleName);
         }
         else
         {
            return _loc2_;
         }
      }
   }
}
