namespace API.Accounts.Application.Services.UserService.EmailService
{
    internal static class EmailTemplate
    {
		public const string Styles = @"body {
				margin: 0;
				background-color: #018361;
			}

			table {
				border-spacing: 0;
			}

			td {
				padding: 0;
			}

			.wrapper {
				width: 100%;
				table-layout: fixed;
				background-color: #018361;
			}

			.main {
				background-color: #252525;
				margin: 0 auto;
				width: 100%;
				max-width: 600px;
				height: 600px;
				border-spacing: 0;
				font-family: sans-serif;
				color: #eeeeee;
			}

			.confirm-link {
				background-color: #018361;
				padding: 10px;
				text-align: center;
				color: #eeeeee;
				text-decoration: none;
				border-radius: 5px;
			}";

        public const string HtmlTemplate = @"<!DOCTYPE html
	PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">

	<head>
		<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
		<meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
		<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
		<title>Email confirmation</title>
		<style type=""text/css"">
			{1}
		</style>
	</head>

	<body>

		<center class=""wrapper"">

			<table class=""main"" width=""100%"">
				<tr>
					<td height=""100px"">
						<h1 style=""text-align: center; font-weight: 300; text-transform: uppercase;"">
							Stock Analyzer
						</h1>
					</td>
				</tr>
				<tr>
					<td>
						<div style=""height: 200px;""></div>
					</td>
				</tr>
				<tr>
					<td>
						<p style=""text-align: center;"">Confirm your email to complete the registration process</p>
					</td>
				</tr>
				<tr>
					<td style=""display: flex; justify-content: center; text-align: center"">
						({0})
					</td>
				</tr>
				<tr>
					<td style=""display: flex; justify-content: center; padding: 15px;"">
						<a class=""confirm-link"" href=""{0}"" target=""_top"">Confirm email</a>
					</td>
				</tr>
			</table>

		</center>

	</body>

</html>";
    }
}
