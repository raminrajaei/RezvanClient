﻿using ATA.HR.Client.Web.APIs.Enums;
using BootstrapBlazor.Components;

namespace ATA.HR.Client.Web.APIs.Models.Request;

public class ChildMoreInfoUpsertDto
{
    public long Id { get; set; } = 0;

    public bool DidChildHaveAccidentInThePast { get; set; }

    /// <summary>
    /// محل آسیب دیدگی 
    /// </summary>
    public string PlaceOfInjury { get; set; }

    /// <summary>
    /// بیماری خاص 
    /// </summary>
    public string SpecialDisease { get; set; }

    /// <summary>
    /// حساسیت دارویی یا غذایی
    /// </summary>
    public string Allergy { get; set; }

    /// <summary>
    /// آیا والدین کودک در قید حیات هستند ؟
    /// </summary>
    public bool AreParentsAlive { get; set; } = true;

    /// <summary>
    /// کودک با چه کسی زندگی میکند ؟
    /// </summary>
    public ChildLiveWithEnum? ChildLiveWith { get; set; }
    public string? ChildLiveWithSelectedValue { get; set; } = ChildLiveWithEnum.Parents.ToString("D");

    /// <summary>
    /// عادات ویژه 
    /// </summary>
    public string SpecialHabits { get; set; } // comma seprator

    /// <summary>
    /// ترسهای خاص 
    /// </summary>
    public string SpecialFears { get; set; }

    /// <summary>
    /// علایق کودک به رنگ
    /// </summary>
    public string ColorInterests { get; set; }

    /// <summary>
    /// علایق کودک به اسباب بازی
    /// </summary>
    public string ToyInterests { get; set; }

    /// <summary>
    /// علایق کودک به ورزش
    /// </summary>
    public string SportInterests { get; set; }

    /// <summary>
    /// علایق کودک به شخصیت های کارتونی و تلویزیونی
    /// </summary>
    public string CharactersInterests { get; set; }

    /// <summary>
    /// شما جهت تشویق و یا تنبیه فرزندتان از چه شیوه ای استفاده میکنید ؟
    /// </summary>
    public string EncouragingOrPunishingWay { get; set; }

    /// <summary>
    /// وضعیت جسمی , حرکتی کودک در مقایسه با کودکان دیگر خانواده چگونه است ؟
    /// </summary>
    public PhysicalConditionEnum? PhysicalCondition { get; set; }
    public string? PhysicalConditionSelectedValue { get; set; }

    /// <summary>
    /// دست غالب
    /// </summary>
    public string WritingHand { get; set; }

    /// <summary>
    /// بیمه
    /// </summary>
    public string Insurance { get; set; }

    /// <summary>
    /// تکمیلی 
    /// </summary>
    public string AdditionalInfo { get; set; }

    /// <summary>
    /// نسبت پدر و مادر 
    /// </summary>
    public string ParentalRatio { get; set; }

    /// <summary>
    /// طریقه آشنایی شما با موسسه
    /// </summary>
    public string FamiliarityInstitution { get; set; }

    /// <summary>
    /// کد ملی معرف
    /// </summary>
    public string IdentifierCode { get; set; }

}