using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitNance.Migrations
{
    public partial class AddMealPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Some FitNance databases already have these tables from the original SQL schema.
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'dbo.MealPlan_H', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[MealPlan_H] (
                        [RowStamp] numeric(18,0) IDENTITY(1,1) NOT NULL,
                        [MealPlanId] varchar(30) NOT NULL,
                        [UserProfileId] varchar(30) NOT NULL,
                        [PlanName] varchar(150) NOT NULL,
                        [TotalCalories] float NULL,
                        [TotalProtein] float NULL,
                        [TotalCarbs] float NULL,
                        [TotalFat] float NULL,
                        [TotalFiber] float NULL,
                        [StartDate] date NULL,
                        [EndDate] date NULL,
                        [CreatedBy] varchar(50) NULL,
                        [CreatedDate] datetime NULL,
                        [ModifiedBy] varchar(50) NULL,
                        [ModifiedDate] datetime NULL,
                        CONSTRAINT [PK_MealPlan_H] PRIMARY KEY ([RowStamp]),
                        CONSTRAINT [AK_MealPlan_H_MealPlanId] UNIQUE ([MealPlanId])
                    );
                END;

                IF OBJECT_ID(N'dbo.MealPlan_D', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[MealPlan_D] (
                        [RowStamp] numeric(18,0) IDENTITY(1,1) NOT NULL,
                        [MealPlanDetailId] varchar(30) NOT NULL,
                        [MealPlanId] varchar(30) NOT NULL,
                        [FoodId] varchar(30) NOT NULL,
                        [MealType] varchar(50) NOT NULL,
                        [Quantity] float NULL,
                        [QuantityGrams] float NULL,
                        [Calories] float NULL,
                        [Protein] float NULL,
                        [Carbs] float NULL,
                        [Fat] float NULL,
                        [Fiber] float NULL,
                        [Sugar] float NULL,
                        [Sodium] float NULL,
                        [MealDate] date NULL,
                        [CreatedBy] varchar(50) NULL,
                        [CreatedDate] datetime NULL,
                        [ModifiedBy] varchar(50) NULL,
                        [ModifiedDate] datetime NULL,
                        CONSTRAINT [PK_MealPlan_D] PRIMARY KEY ([RowStamp]),
                        CONSTRAINT [FK_MealPlan_D_MealPlan_H_MealPlanId]
                            FOREIGN KEY ([MealPlanId]) REFERENCES [dbo].[MealPlan_H] ([MealPlanId])
                    );

                    CREATE UNIQUE INDEX [IX_MealPlan_D_MealPlanDetailId]
                        ON [dbo].[MealPlan_D] ([MealPlanDetailId]);
                    CREATE INDEX [IX_MealPlan_D_MealPlanId] ON [dbo].[MealPlan_D] ([MealPlanId]);
                END;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new NotSupportedException(
                "AddMealPlan may have adopted existing Meal Plan tables. Rollback would risk deleting their data.");
        }
    }
}
