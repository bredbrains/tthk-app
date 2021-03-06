﻿using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        private async void VkButton_OnClicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://vk.com/tthkbot");
        }

        private async void GitHubButton_OnClicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://github.com/bredbrains/tthk-app");
        }
        private async void ImageTapped(object sender, EventArgs args)
        {
            try
            {
                await Browser.OpenAsync("https://bredbrains.tech/");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void SchoolLinkLabelTapped(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync("https://tthk.ee/");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}