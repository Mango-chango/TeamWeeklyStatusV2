// Import the AWS SDK and Axios
import axios from 'axios';

// Lambda handler function
// SendReminderSendWeeklyReport
export const handler = async (event) => {
    try {
        // Define the endpoint URL
        const url = 'https://misha24.azurewebsites.net//api/WeeklyStatus/SendReminders';

        // Make the POST request to the provided endpoint
        const payload = { "eventName": "SendReport"};
        const response = await axios.post(url, body, {
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload),
        });
        
        // Extract the data from the response
        const data = response.data;

        // Log the data to CloudWatch
        console.log('Returned Data:', data);

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
                message: 'Error sending reminder',
                error: error.message
            })
        };
    }
};