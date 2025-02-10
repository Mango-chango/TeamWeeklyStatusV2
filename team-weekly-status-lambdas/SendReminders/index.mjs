// Import the AWS SDK and Axios
import axios from 'axios';

// Lambda handler function
export const handler = async (event) => {
    try {
        // Define the endpoint URL
        const url = 'https://misha24.azurewebsites.net/api/v2.0/WeeklyStatus/SendReminders';

        // Make the POST request to the provided endpoint
        let payload = {
            eventName: "Post"
        };
        const response = await axios.post(url, payload);
        
        // Extract the data from the response
        const data = response.data;

        // Log the data to CloudWatch
        console.log('Fetched Data from Post event:', data);
        
        payload = {
            eventName: "SendReport"
        }
        const responseSendReport = await axios.post(url, payload);
        
        // Extract the data from the response
        const dataSendReport = responseSendReport.data;

        // Log the data to CloudWatch
        console.log('Fetched Data from SendReport event:', dataSendReport);

        // Return a successful response
        return {
            statusCode: 200,
            body: JSON.stringify(data)
        };
    } catch (error) {
        // Log the error to CloudWatch
        console.error('Error:', error);

        // Return an error response if something goes wrong
        return {
            statusCode: error.response?.status || 500,
            body: JSON.stringify({
                message: 'Error fetching data',
                error: error.message
            })
        };
    }
};