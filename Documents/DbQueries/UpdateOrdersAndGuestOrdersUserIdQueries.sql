-- Update user info in FoodDetailOrder Table (guest orders) based on UsersView

Update [ATA].[FoodDetailOrder]
   Set [ATA].[FoodDetailOrder].UserId = [ATA].[UsersView].UserId,
   	   [ATA].[FoodDetailOrder].UserFullName = [ATA].[UsersView].FullName ,
   	   [ATA].[FoodDetailOrder].UserWorkLocation = [ATA].[UsersView].WorkLocation,
   	   [ATA].[FoodDetailOrder].UserUnit = [ATA].[UsersView].UnitName
   From [ATA].[FoodDetailOrder]
	   INNER JOIN [ATA].[UsersView] On [ATA].[FoodDetailOrder].EmployeeCode = [ATA].[UsersView].PersonnelCode