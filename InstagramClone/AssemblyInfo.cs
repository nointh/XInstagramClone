using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Android.App;

[assembly: ExportFont("fa-brands-400.ttf", Alias = "FABrands")]
[assembly: ExportFont("fa-regular-400.ttf", Alias = "FARegular")]
[assembly: ExportFont("fa-solid-400.ttf", Alias = "FASolid")]
[assembly: ExportFont("FontAwesome5Free-Solid-900.otf", Alias = "FFASolid")]
[assembly: ExportFont("FontAwesome5Free-Regular-400.otf", Alias = "FFARegular")]
[assembly: ExportFont("FontAwesome5Brands-Regular-400.otf", Alias = "FFABrands")]
[assembly: ExportFont("FontAwesome5Pro-Regular-400.otf", Alias = "PFARegular")]
[assembly: ExportFont("FontAwesome5Pro-Light-300.otf", Alias = "PFALight")]
[assembly: ExportFont("FontAwesome5Pro-Solid-900.otf", Alias = "PFASolid")]
[assembly: ExportFont("FontAwesome5Duoton-Solid-400.otf", Alias = "PFADuotone")]

// Needed for Picking photo/video
[assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage)]
// Needed for Taking photo/video
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Android.Manifest.Permission.Camera)]
// Add these properties if you would like to filter out devices that do not have cameras, or set to false to make them optional
[assembly: UsesFeature("android.hardware.camera", Required = true)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = true)]

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]