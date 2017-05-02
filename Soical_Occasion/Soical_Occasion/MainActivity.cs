using Android.App;
using Android.OS;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Geolocator;
using Geolocator.Plugin;
using System;
using System.Threading;
using Android.Locations;
using Android.Runtime;
using Android.Content;
using Android.Content.PM;
namespace Soical_Occasion
{
    [Activity(Label = "Soical_Occasion", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        GoogleMap _googleMap;
        MapFragment _mapFragment;
        LocationManager _locationManager;
        Location _currentLocation;
        string _provider;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            SetUpMap();
            SetUpLocation();
        }

        protected override void OnResume()
        {
            _locationManager.RequestLocationUpdates(_provider, 0, 0, this);
            base.OnResume();
        }

        protected override void OnPause()
        {
            _locationManager.RemoveUpdates(this);
            base.OnPause();
        }

        public void SetUpMap()
        {

            if (_mapFragment == null)
            {
                _mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
                _mapFragment.GetMapAsync(this);
            }


            //_mapFragment.GetMapAsync(this);

            //if (_googleMap != null)
            //{
            //    _googleMap.UiSettings.ZoomControlsEnabled = true;
            //    _googleMap.UiSettings.CompassEnabled = true;
            //    _googleMap.UiSettings.MyLocationButtonEnabled = true;
            //}
        }

        public void SetUpLocation()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine,
                PowerRequirement = Power.Medium
            };

            _provider = _locationManager.GetBestProvider(new Criteria(), true);

            if (_provider == null)
            {
                OnProviderDisabled(_provider);
            }

            _currentLocation = _locationManager.GetLastKnownLocation(_provider);
            if (_currentLocation != null)
            {
                OnLocationChanged(_currentLocation);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;

            _googleMap.UiSettings.ZoomControlsEnabled = true;
            _googleMap.UiSettings.CompassEnabled = true;
        }

        public void AddMarker(string name, LatLng latlng)
        {
            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(latlng);
            markerOpt1.SetTitle(name);
            _googleMap.AddMarker(markerOpt1);
        }

        async System.Threading.Tasks.Task<LatLng> GetCurrentLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                if (position == null)
                    return null;
                
                return new LatLng(position.Latitude, position.Longitude);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                return null;
            }
        }

        public void OnLocationChanged(Location location)
        {
            LatLng latlng = new LatLng(location.Latitude, location.Longitude);

            if (_googleMap != null)
            {
                _googleMap.MoveCamera(CameraUpdateFactory.NewLatLng(latlng));
                _googleMap.AnimateCamera(CameraUpdateFactory.ZoomTo(10));
                AddMarker("my position, location", latlng);
            }
            //_googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(location.Latitude, location.Longitude), 1));
            //throw new NotImplementedException();
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }
    }
}