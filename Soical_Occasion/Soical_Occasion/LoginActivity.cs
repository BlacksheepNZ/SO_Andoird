using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;

using System.Collections.Specialized;
using Newtonsoft.Json;
using System;

namespace Soical_Occasion
{
    [Activity(Label = "Soical_Occasion", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
    public class LoginActivity : Activity
    {
        private Button _buttonSignUp;
        private Button _ButtonLogin;

        private EditText _login_username;
        private EditText _login_password;

        private ProgressBar _progressbar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
             SetContentView (Resource.Layout.index);

            _progressbar = FindViewById<ProgressBar>(Resource.Id.loadingbar);

            _login_username = FindViewById<EditText>(Resource.Id.inputUsername);
            _login_password = FindViewById<EditText>(Resource.Id.inputPassword);

            _ButtonLogin = FindViewById<Button>(Resource.Id.buttonLoginIn);
            _ButtonLogin.Click += (object sender, EventArgs e) =>
            {
                _progressbar.Visibility = ViewStates.Visible;

                RequestLoginInformation();
            };

            _buttonSignUp = FindViewById<Button>(Resource.Id.signupbutton);
            _buttonSignUp.Click += (object sender, EventArgs e) =>
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                dialog_signup signUpDialog = new dialog_signup();
                signUpDialog.Show(transaction, "dialog fragment");
                
                signUpDialog._onSignUpComplete += SignUpDialog__onSignUpComplete;
            };
        }

        private void SignUpDialog__onSignUpComplete(object sender, OnSignUpEventArgs e)
        {
            _progressbar.Visibility = ViewStates.Visible;

            // request params
            string apiUrl = Settings.apiUrl;
            string apiMethod = "registerUser";
            Register_Request myRegister_Request = new Register_Request()
            {
                username = e.Username,
                password = e.Password,
                email = e.Email
            };

            // make http post request
            string response = Http.Post(apiUrl, new NameValueCollection()
            {
                { "api_method", apiMethod                                    },
                { "api_data",   JsonConvert.SerializeObject(myRegister_Request) }
            });

            // decode json string to dto object
            API_Response r =
                JsonConvert.DeserializeObject<API_Response>(response);

            // check response
            if (!r.IsError && r.ResponseData == "SUCCESS")
            {
                _login_username.Text = e.Username;
                _login_password.Text = e.Password;
                Toast.MakeText(this, r.ErrorMessage, ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, r.ErrorMessage, ToastLength.Long).Show();
            }

            _progressbar.Visibility = ViewStates.Invisible;
        }

        //request thread
        private void RequestLoginInformation()
        {
            // request params
            string apiUrl = Settings.apiUrl;
            string apiMethod = "loginUser";
            Login_Request myLogin_Request = new Login_Request()
            {
                username = _login_username.Text,
                password = _login_password.Text
            };

            // make http post request
            string response =  Http.Post(apiUrl, new NameValueCollection()
            {
                { "api_method", apiMethod                                    },
                { "api_data",   JsonConvert.SerializeObject(myLogin_Request) }
            });

            // decode json string to dto object
            API_Response r =
                JsonConvert.DeserializeObject<API_Response>(response);

            // check response
            if (!r.IsError && r.ResponseData == "SUCCESS")
            {
                StartActivity(typeof(MainActivity));
            }
            else
            {
                Toast.MakeText(this, r.ErrorMessage, ToastLength.Long).Show();
            }

            _progressbar.Visibility = ViewStates.Invisible;
        }
    }
}

