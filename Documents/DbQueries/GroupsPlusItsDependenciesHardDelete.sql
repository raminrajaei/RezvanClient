-- Hard Delete Groups

DECLARE @GroupId AS bigint
SET @GroupId = 309

--1. Delete Related Orders   
Delete ATA.HROrder Where Id In (Select fo.Id From ATA.HROrder As fo
Left Join ATA.GroupCalendar As gc On gc.Id = fo.GroupCalendarId
Left Join ATA.[Group] As g On g.Id = gc.GroupId
Where gc.GroupId = @GroupId)

--2. Delete Related GuestOrders
Delete ATA.HRDetailOrder Where Id In (Select fo.Id From ATA.HRDetailOrder As fo
Left Join ATA.GroupCalendar As gc On gc.Id = fo.GroupCalendarId
Left Join ATA.[Group] As g On g.Id = gc.GroupId
Where gc.GroupId = @GroupId)

--3. Delete Related Comments
Delete ATA.CommentOrder Where Id In (Select c.Id From ATA.CommentOrder As c
Left Join ATA.GroupCalendar As gc On gc.Id = c.GroupCalendarId
Left Join ATA.[Group] As g On g.Id = gc.GroupId
Where gc.GroupId = @GroupId)

--4. Delete FoodGroupCalendar
Delete ATA.HRGroupCalendar Where Id In (Select fgc.Id From ATA.HRGroupCalendar as fgc
Left Join ATA.GroupCalendar As gc On gc.Id = fgc.GroupCalendarId
Left Join ATA.[Group] As g On g.Id = gc.GroupId
Where gc.GroupId = @GroupId)

--5. Delete GroupCalendar
Delete ATA.GroupCalendar Where Id In (Select gc.Id From ATA.GroupCalendar As gc
Left Join ATA.[Group] As g On g.Id = gc.GroupId
Where gc.GroupId = @GroupId)

--6. Delete Group finally
Delete ATA.[Group] Where Id = @GroupId