using ATA.HR.Client.Web.APIs.Enums;

namespace ATA.HR.Client.Web.APIs.Models.Request;

public class AdultMoreInfoUpsertDto
{
    public long Id { get; set; }

    /// <summary>
    /// آيا تاكنون در دوره تربیت معلم قرآن‌كريم شركت داشته‌ايد؟ 
    /// </summary>
    public bool ParticipateInQuran { get; set; }

    /// <summary>
    /// در چه رشته‌اي؟
    /// </summary>
    public string ParticipateField { get; set; }

    /// <summary>
    /// در چه سالي ؟
    /// </summary>
    public int ParticipateYear { get; set; }

    /// <summary>
    /// دركدام استان؟
    /// </summary>
    public string ParticipateProvince { get; set; }

    /// <summary>
    /// آيا تا به حال به عنوان مدرّس يا معلّم، جلسه آموزش قرآن داشته‌ايد؟ 
    /// </summary>
    public bool ParticipateInQuranAsTeacher { get; set; }

    /// <summary>
    /// درچه سطحي؟
    /// </summary>
    public string ParticipateInQuranAsTeacherLevel { get; set; }

    /// <summary>
    /// درچه زماني ؟
    /// </summary>
    public DateTime? ParticipateInQuranAsTeacherDate { get; set; }

    /// <summary>
    /// آيا هم‌اكنون در جلسه آموزش قرائت، مفاهيم يا حفظ  قرآن كريم تدريس داريد
    /// </summary>
    public bool CurrentlyTeachingRecitation { get; set; }

    /// <summary>
    /// ‌مشخصات جلسه خود را بنويسيد
    /// </summary>
    public string SessionDetails { get; set; }

    /// <summary>
    /// ميزان آشنايي شما با زبان عربي و علوم قرآني تا چه سطحي مي‌باشد؟ 
    /// </summary>
    public FamiliarWithArabicAndQuranEnum FamiliarWithArabicAndQuran { get; set; }

    /// <summary>
    /// چه كتابهايي در اين زمينه ‌خوانده‌ايد؟
    /// </summary>
    public string WhatBooks { get; set; }
}