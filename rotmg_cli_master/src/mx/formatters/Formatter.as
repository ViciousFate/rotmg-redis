package mx.formatters
{
   import mx.core.mx_internal;
   import mx.resources.IResourceManager;
   import mx.resources.ResourceManager;
   import flash.events.Event;
   
   use namespace mx_internal;
   
   public class Formatter extends Object implements IFormatter
   {
      
      public function Formatter() {
         this._resourceManager = ResourceManager.getInstance();
         super();
         this.resourceManager.addEventListener(Event.CHANGE,this.resourceManager_changeHandler,false,0,true);
         this.resourcesChanged();
      }
      
      mx_internal  static const VERSION:String = "4.6.0.23201";
      
      private static var initialized:Boolean = false;
      
      private static var _static_resourceManager:IResourceManager;
      
      private static function get static_resourceManager() : IResourceManager {
         if(!_static_resourceManager)
         {
            _static_resourceManager = ResourceManager.getInstance();
         }
         return _static_resourceManager;
      }
      
      private static var _defaultInvalidFormatError:String;
      
      private static var defaultInvalidFormatErrorOverride:String;
      
      public static function get defaultInvalidFormatError() : String {
         initialize();
         return _defaultInvalidFormatError;
      }
      
      public static function set defaultInvalidFormatError(param1:String) : void {
         defaultInvalidFormatErrorOverride = param1;
         _defaultInvalidFormatError = param1 != null?param1:static_resourceManager.getString("formatters","defaultInvalidFormatError");
      }
      
      private static var _defaultInvalidValueError:String;
      
      private static var defaultInvalidValueErrorOverride:String;
      
      public static function get defaultInvalidValueError() : String {
         initialize();
         return _defaultInvalidValueError;
      }
      
      public static function set defaultInvalidValueError(param1:String) : void {
         defaultInvalidValueErrorOverride = param1;
         _defaultInvalidValueError = param1 != null?param1:static_resourceManager.getString("formatters","defaultInvalidValueError");
      }
      
      private static function initialize() : void {
         if(!initialized)
         {
            static_resourceManager.addEventListener(Event.CHANGE,static_resourceManager_changeHandler,false,0,true);
            static_resourcesChanged();
            initialized = true;
         }
      }
      
      private static function static_resourcesChanged() : void {
         defaultInvalidFormatError = defaultInvalidFormatErrorOverride;
         defaultInvalidValueError = defaultInvalidValueErrorOverride;
      }
      
      private static function static_resourceManager_changeHandler(param1:Event) : void {
         static_resourcesChanged();
      }
      
      public var error:String;
      
      private var _resourceManager:IResourceManager;
      
      protected function get resourceManager() : IResourceManager {
         return this._resourceManager;
      }
      
      protected function resourcesChanged() : void {
      }
      
      public function format(param1:Object) : String {
         this.error = "This format function is abstract. " + "Subclasses must override it.";
         return "";
      }
      
      private function resourceManager_changeHandler(param1:Event) : void {
         this.resourcesChanged();
      }
   }
}
