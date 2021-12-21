using System;
using System.Collections.Generic;
using System.Text;

namespace TinCore.Common.Enums
{
    public enum eRecordStatus : byte
    {
        InActive = 0,
        Active = 1
    }
    public enum eLocationType : int
    {
        Country = 1384,
        Region = 1385,
        City = 1386,
        Neighbourhood = 1387
    }
    public enum eProfileType : short
    {
        Attribute = 8020,
        Review = 8021,
        Section = 8022
    }
    public enum eProfileSubType : short
    {
        Profile1 = 1,
        Profile2 = 2,
    }
    public enum eContactType : short
    {
        Primary = 8100,
        Business = 8114
    }

    public enum eEoTypes : short
    {
        ENTITY = 1010,   /// BUSINESS
        COMPANY = 1011,
        LOGON = 1012,
        CUSTOMER = 1020,
        ROLE = 1030,
        SALES_ORDER = 2260,
        APP_PRG = 2270,
        APP_PRG_FIELD = 2271,
        TEMPLATE = 11,
        PRODUCT = 2050,
        SUBSCRIPTION = 2051,
        ISSUE = 2290,
        PAGE_BLOCK = 2340,
        LOCATION = 2410,
        LOCATIONTYPE = 2411,
        LOCATION_PROGRAM = 2412,
        CLASSIFICATION = 2420,
        CLASSIFICATIONTYPE = 2421,
        REVIEW_CATEGORY_UL = 2424,
        REVIEW_CATEGORY_LI = 2425,
        ATTRIBUTE = 2430,
        GALLERY = 2431,
        IMAGE = 2432,
        VIDEO = 2433,
        FOLDER = 2434,
        FILE = 2435,
        LIST = 2530,
        LIST_ITEM = 2531,

        RELATION_RELATION = 2700, // location_category_id______business_id
        EO_ATTRIBUTE = 2711,
        LOCATION_EO = 2712,
        LOCATION_CATEGORY = 2710
    }


    public enum eEoSubTypes : short
    {
        INSTANCE = 12,
        CLASS = 10
    }
    public enum eRelationTypes : short
    {
        RELATION_RELATION = 2700, // location_category_id______business_id
        EO_ATTRIBUTE = 2711,
        LOCATION_EO = 2712,
        LOCATION_CATEGORY = 2710
    }

    public enum eTrackingType : short
    {
        TRACKING_PRICE = 3040,
        TRACKING_COUPON = 3032,
        EO_TYPE_SUBSCRIPTION = 2051,
        TRACKING_TICKET = 3033,
        TRACKING_EVENT = 3031
    }
    public enum eIncludeOptions: short
    {
        Slim,
        Default,
        All,
        Contact,
        ProfileAttribute,
        ProfileSection,
        ProfileReview,
        Tracking
    }

    public enum eTrackingSubType : short
    {
        
    }

}
