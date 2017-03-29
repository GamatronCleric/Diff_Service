Written in Visual Studio Express 2017.
Prerequisites: - Sql Server 2016 Express Edition Installed
			   - Start Visual Studio as Administrator

Start the DifferService.
Set Diff_Service as startup Project (if this is not the case).

Use a tool like Postman: https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop?hl=en
to create the API calls which are required for this service.

Create a PUT request for LEFT data with the following url:
http://localhost:8733/DifferService/v1/diff/1/left
Add a body in raw/JSON(application/json format) (i.e)
{
	"Data":"AAA="
}

Create a PUT request for RIGHT data with the following url:
http://localhost:8733/DifferService/v1/diff/1/right
Add a body in raw/JSON(application/json format) (i.e)
{
	"Data":"AAA="
}

Create a GET request with the following URL to retrieve the results:
http://localhost:8733/DifferService/v1/diff/1