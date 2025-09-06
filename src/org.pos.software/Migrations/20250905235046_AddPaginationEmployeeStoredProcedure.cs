using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace org.pos.software.Migrations
{
    /// <inheritdoc />
    public partial class AddPaginationEmployeeStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE getEmployeePagination 
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
                        ROW_NUMBER() OVER(ORDER BY e.dni ASC) AS Fila,
                        COUNT(*) OVER() AS TotalFilas
                    FROM tbl_employee e
	                WHERE e.is_deleted = 0
                    ORDER BY e.first_name ASC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY
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
