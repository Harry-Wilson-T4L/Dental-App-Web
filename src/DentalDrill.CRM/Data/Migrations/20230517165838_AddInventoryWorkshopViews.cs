using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddInventoryWorkshopViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create or alter view [dbo].[InventorySKUsWorkshopQuantity] ([Id], [WorkshopId], [TotalQuantity], [ShelfQuantity], [TrayQuantity], [OrderedQuantity], [RequestedQuantity], [RepairQuantity])
as
select 
  ascendants.[AscendantId] as [Id],
  movs.[WorkshopId],
  sum(case when [Status] in (100) then [Quantity] else 0 end) as [TotalQuantity],
  sum(case when [Status] in (90, 100) then [Quantity] else 0 end) as [ShelfQuantity],
  sum(case when [Status] in (90) then [QuantityAbsolute] else 0 end) as [TrayQuantity],
  sum(case when [Status] in (30) then [Quantity] else 0 end) as [OrderedQuantity],
  sum(case when [Status] in (10, 20) then [Quantity] else 0 end) as [RequestedQuantity],
  sum(case when [Type] in (1000) and [Status] in (40,90) then [QuantityAbsolute] else 0 end) as [RepairQuantity]
from [InventoryMovements] movs
inner join [InventorySKUs] sku on movs.[SKUId] = sku.[Id]
inner join [InventorySKUsAscendants] ascendants on movs.[SKUId] = ascendants.[DescendantId]
where sku.[DeletionStatus] = 0
group by ascendants.[AscendantId], movs.[WorkshopId]");

            migrationBuilder.Sql(@"create or alter view [dbo].[InventorySKUsWorkshopRequiredQuantity] ([Id], [WorkshopId], [RequiredQuantity])
as
with [GroupFinalChild] ([Id], [ChildParent], [DefaultChildId])
as
(
    select sku.[Id], sku.[Id], sku.[DefaultChildId]
	from [InventorySKUs] sku
	where sku.[NodeType] = 1 and sku.[DeletionStatus] = 0 and sku.[DefaultChildId] is not null
	union all
	select q.[Id], sku.[Id], sku.[DefaultChildId]
	from [GroupFinalChild] q
	inner join [InventorySKUs] sku on q.[DefaultChildId] = sku.[Id] and q.[ChildParent] = sku.[ParentId] and sku.[DeletionStatus] = 0 and sku.[DefaultChildId] is not null
), [SKUFinalChild] ([SourceId], [FinalId])
as
(
    select [Id] as [SourceId], [Id] as [FinalId] from [InventorySKUs] sku
    where sku.[NodeType] = 0 and sku.[DeletionStatus] = 0
    union all
    select fc.[DefaultChildId], fc.[Id] from [GroupFinalChild] fc inner join [InventorySKUs] sku on fc.[DefaultChildId] = sku.[Id] and sku.[NodeType] = 0 and sku.[DeletionStatus] = 0
)
select fc.[SourceId] as [Id], w.[Id] as [WorkshopId], max(coalesce(skuw.[WarningQuantity], 0)) as [RequiredQuantity]
from [SKUFinalChild] fc
inner join [InventorySKUs] sku on fc.[FinalId] = sku.[Id]
cross join [Workshops] w
left join [InventorySKUWorkshopOptions] skuw on skuw.[SKUId] = sku.[Id] and skuw.[WorkshopId] = w.[Id]
group by fc.[SourceId], w.[Id]");

            migrationBuilder.Sql(@"create or alter view [dbo].[InventorySKUsWorkshopMissingQuantity] ([Id], [WorkshopId], [MissingQuantity])
as
select
  sku.[Id],
  w.[Id] as [WorkshopId],
  coalesce(rq.[RequiredQuantity], 0) - (coalesce(q.[ShelfQuantity], 0) + coalesce(q.[OrderedQuantity], 0) + coalesce(q.[RequestedQuantity], 0)) as [MissingQuantity]
from [InventorySKUs] sku
cross join [Workshops] w
left join [InventorySKUsWorkshopQuantity] q on q.[Id] = sku.[Id] and q.[WorkshopId] = w.[Id]
left join [InventorySKUsWorkshopRequiredQuantity] rq on rq.[Id] = sku.[Id] and rq.[WorkshopId] = w.[Id]
where sku.[NodeType] = 0 and sku.[DeletionStatus] = 0
  and (coalesce(rq.[RequiredQuantity], 0) - (coalesce(q.[ShelfQuantity], 0) + coalesce(q.[OrderedQuantity], 0) + coalesce(q.[RequestedQuantity], 0))) > 0");

            migrationBuilder.Sql(@"create or alter view [dbo].[InventorySKUsWorkshopWarnings] ([Id], [WorkshopId], [HasWarning])
as
select 
  sku.[Id],
  w.[Id],
  case when skuw.[WarningQuantity] is not null and skuw.[WarningQuantity] > coalesce(q.[TotalQuantity], 0) and sku.[DeletionStatus] = 0 then 1 else 0 end as [HasWarning]
from [InventorySKUs] sku
cross join [Workshops] w
left join [InventorySKUWorkshopOptions] skuw on skuw.[SKUId] = sku.[Id] and skuw.[WorkshopId] = w.[Id]
left join [InventorySKUsWorkshopQuantity] q on sku.[Id] = q.[Id] and w.[Id] = q.[WorkshopId]");

            migrationBuilder.Sql(@"create or alter view [dbo].[InventorySKUsWorkshopDescendantsWarnings] ([Id], [WorkshopId], [HasDescendantsWithWarning])
as
select 
  a.[AscendantId] as [Id],
  w.[WorkshopId],
  max(w.[HasWarning]) as [HasDescendantsWithWarning]
from [InventorySKUsWorkshopWarnings] w
inner join [InventorySKUsAscendants] a on w.[Id] = a.[DescendantId]
group by a.[AscendantId], w.[WorkshopId]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop view if exists [dbo].[InventorySKUsWorkshopMissingQuantity]");

            migrationBuilder.Sql(@"drop view if exists [dbo].[InventorySKUsWorkshopRequiredQuantity]");

            migrationBuilder.Sql(@"drop view if exists [dbo].[InventorySKUsWorkshopQuantity]");

            migrationBuilder.Sql(@"drop view if exists [dbo].[InventorySKUsWorkshopWarnings]");

            migrationBuilder.Sql(@"drop view if exists [dbo].[InventorySKUsWorkshopDescendantsWarnings]");
        }
    }
}
