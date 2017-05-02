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
using Android.Graphics;

namespace Soical_Occasion
{
    [Activity(Label = "Soical_Occasion", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        private GoogleMap _googleMap;
        private MapFragment _mapFragment;
        private LocationManager _locationManager;
        private Location _currentLocation;
        private string _provider;

        private Accuracy _accuracy = Accuracy.Fine;
        private Power _power = Power.Medium;

        private float _cameraZoom = 18.0f;
        GroundOverlay _myOverlay;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            SetUpMap();
        }

        protected override void OnResume()
        {
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
        }

        public void SetUpLocation()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = _accuracy,
                PowerRequirement = _power
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

            _locationManager.RequestLocationUpdates(_provider, 0, 0, this);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;

            googleMap.MyLocationEnabled = true;
            googleMap.MyLocationButtonClick += LocationButtonClick;

            googleMap.UiSettings.MapToolbarEnabled = false;
            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;

            SetUpLocation();

            //Set up current position/image
            BitmapDescriptor image = BitmapDescriptorFactory.FromResource(Resource.Drawable.bluedot);
            GroundOverlayOptions groundOverlayOptions = new GroundOverlayOptions()
                .Position(LocationToLatLong(_currentLocation), 10, 10)
                .InvokeImage(image);
            _myOverlay = _googleMap.AddGroundOverlay(groundOverlayOptions);

            MoveCamera(_currentLocation);
        }

        private void LocationButtonClick(object sender, GoogleMap.MyLocationButtonClickEventArgs e)
        {
            MoveCamera(_currentLocation);
        }

        public void AddMarker(string name, LatLng latlng)
        {
            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(latlng);
            markerOpt1.SetTitle(name);
            _googleMap.AddMarker(markerOpt1);
        }

        public void OnLocationChanged(Location location)
        {
            MoveCamera(location);
        }

        private void MoveCamera(Location location)
        {
            if (_googleMap != null)
            {
                _googleMap.MoveCamera(CameraUpdateFactory.NewLatLng(LocationToLatLong(location)));
                _googleMap.AnimateCamera(CameraUpdateFactory.ZoomTo(_cameraZoom));

                if (_myOverlay != null)
                {
                    _myOverlay.Position = LocationToLatLong(location);
                }
            }
        }

        private LatLng LocationToLatLong(Location location)
        {
            return new LatLng(location.Latitude, location.Longitude);
        }

        #region Unused

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

        #endregion
    }
}