﻿namespace SchoolLineup.Web.Mvc
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            // Set this to true, enables minification doesn't matter what is configured in Web.config file in compilation section
            // In the Web.config file, section compilation, while debug='true' then minification is disabled
            BundleTable.EnableOptimizations = true;

            #region AllFiles

            //bundles.Add(new ScriptBundle("~/bundles/app").Include(
            //            "~/Scripts/jquery-{version}.js",
            //            "~/Scripts/underscore*",
            //            "~/Scripts/knockout-{version}.js"));

            #endregion

            #region JQuery

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-2.1.0.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery-validate").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*",
            //            "~/Scripts/App/Extensions/Validation*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-mask").Include(
                        "~/Scripts/jquery.maskedinput*",
                        "~/Scripts/App/Masks*"));

            bundles.Add(new ScriptBundle("~/bundles/string").Include(
                        "~/Scripts/string*"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery-dateFormat").Include(
            //            "~/Scripts/jquery.dateFormat-{version}.js"));

            #endregion JQuery

            #region Knockout And App Scripts

            bundles.Add(new ScriptBundle("~/bundles/app/default").IncludeDirectory(
                "~/Scripts/App",
                "Default*",
                true));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/bundles/ko/school").IncludeDirectory(
                "~/Scripts/ViewModels/",
                "School*"));

            bundles.Add(new ScriptBundle("~/bundles/ko/course")
                .Include("~/Scripts/ViewModels/CourseViewModel.js"));

            bundles.Add(new ScriptBundle("~/bundles/ko/college").IncludeDirectory(
                "~/Scripts/ViewModels/",
                "College*"));

            bundles.Add(new ScriptBundle("~/bundles/ko/student").IncludeDirectory(
                "~/Scripts/ViewModels/",
                "Student*"));

            bundles.Add(new ScriptBundle("~/bundles/ko/coursedashboard")
                .Include("~/Scripts/ViewModels/CourseDashboardViewModel.js"));

            bundles.Add(new ScriptBundle("~/bundles/ko/examresult")
                .Include("~/Scripts/ViewModels/ExamResultViewModel.js"));

            bundles.Add(new ScriptBundle("~/bundles/ko/teacher")
                .Include("~/Scripts/ViewModels/TeacherViewModel.js"));

            bundles.Add(new ScriptBundle("~/bundles/ko/user")
                .Include("~/Scripts/ViewModels/UserViewModel.js"));

            bundles.Add(new ScriptBundle("~/bundles/ko/changepassword")
                .Include("~/Scripts/ViewModels/ChangePasswordViewModel.js"));

            #endregion Knockout And App Scripts

            #region Underscore

            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
                "~/Scripts/underscore-min.js"));

            #endregion Underscore

            #region Modernizr

            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            #endregion Modernizr

            #region Styles

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include("~/Content/themes/base/jquery.ui.all.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/default.css"));

            bundles.Add(new StyleBundle("~/Content/css/login").Include(
                "~/Content/login.css"));

            //bundles.Add(new StyleBundle("~/Content/css/job-opportunity").Include("~/Content/JobOpportunity.css"));

            #endregion Styles
        }
    }
}