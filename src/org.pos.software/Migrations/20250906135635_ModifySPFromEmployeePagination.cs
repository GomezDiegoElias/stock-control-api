using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace org.pos.software.Migrations
{
    /// <inheritdoc />
    public partial class ModifySPFromEmployeePagination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('getEmployeePagination', 'P') IS NOT NULL
                    DROP PROCEDURE getEmployeePagination;
                GO

                CREATE PROCEDURE getEmployeePagination 
                    @Dni INT = NULL,
                    @FirstName NVARCHAR(100) = NULL,
                    @LastName NVARCHAR(100) = NULL,
                    @Workstation NVARCHAR(100) = NULL,
                    @PageIndex INT = 1,
                    @PageSize INT = 10
                AS
                BEGIN
                    DECLARE @Offset INT = (@PageSize * (@PageIndex - 1));

                    SELECT
                        e.dni,
                        e.first_name,
                        e.last_name,
                        e.workstation,
                        ROW_NUMBER() OVER(ORDER BY e.first_name ASC) AS Fila,
                        COUNT(*) OVER() AS TotalFilas
                    FROM tbl_employee e
                    WHERE e.is_deleted = 0 AND
                          (@Dni IS NULL OR e.dni = @Dni) AND
                          (@FirstName IS NULL OR e.first_name LIKE '%' + @FirstName + '%') AND
                          (@LastName IS NULL OR e.last_name LIKE '%' + @LastName + '%') AND
                          (@Workstation IS NULL OR e.workstation LIKE '%' + @Workstation + '%')
                    ORDER BY e.first_name ASC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS getEmployeePagination");
        }
    }
}
