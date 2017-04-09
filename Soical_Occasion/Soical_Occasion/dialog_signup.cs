using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Collections.Specialized;
using Newtonsoft.Json;

namespace Soical_Occasion
{
    public class OnSignUpEventArgs : EventArgs
    {
        private string _username;
        private string _email;
        private string _password;

        public string Username
        {
          get { return _username; }
          set { _username = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public OnSignUpEventArgs(string username, string email, string password) : base()
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }

    class dialog_signup : DialogFragment
    {
        private EditText _txtUsername;
        private EditText _txtEmail;
        private EditText _txtPassword;
        private Button _signUpButton;

        public event EventHandler<OnSignUpEventArgs> _onSignUpComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_sign_up, container, false);

            //retrive user input
            _txtUsername = view.FindViewById<EditText>(Resource.Id.txtFirstName);
            _txtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            _txtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            _signUpButton = view.FindViewById<Button>(Resource.Id.btnDialogEmail);

            _signUpButton.Click += (object sender, EventArgs e) =>
            {
                //user click click signup button
                _onSignUpComplete(sender,
                    new OnSignUpEventArgs(_txtUsername.Text, _txtEmail.Text, _txtPassword.Text));
                this.Dismiss();
            };

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}