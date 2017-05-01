using Android.App;
using Android.OS;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Geolocator;
using Geolocator.Plugin;
using System;

namespace Soical_Occasion
{
    [Activity(Label = "Soical_Occasion", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity, IOnMapReadyCallback
    {
        GoogleMap _googleMap;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            SetUpMap();
        }

        public void SetUpMap()
        {
            if(_googleMap ==null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;
            _googleMap.MapType = GoogleMap.MapTypeNormal;
            //_googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(40.776408, -73.970755), 10));
            //_googleMap.AddMarker(new MarkerOptions().SetPosition(new LatLng(40.776408, -73.970755)).SetTitle("new york"));
        }
    }
}