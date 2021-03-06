## Service for comparison Binary Data

It contains three HTTP endpoints:

 * (POST) 'host'/v1/diff/'id'/left - accepts JSON Base64 encoded binary data
 * (POST) 'host'/v1/diff/'id'/right - accepts JSON Base64 encoded binary data
 * (GET)  'host'/v1/diff/'id' - returns JSON result 

 GET method returns three possible results:
 
  * Equal - if byte arrays are the same.
  * Not equal - if data length in arrays are different.
  * Same size with offset - if data length are the same, but the data itself are different. Result is JSON object with value of data length, offset indexes and result status string.
  
  ### Usage
  
  WAES.WebApi.SelfHost.exe
  
  Self hosted WEB.API application will be started. Access is possible on http://localhost:9000/
  
  Steps to use:
  
  * Posting value on the Left endpoint with desired ID will store binary data into temporary storage.
  * Posting value on the Right endpoint with desired ID will store binary data into temporary storage.
  * Calling GET method for desired ID will retrieve result object in JSON.
  
  ### Suggestions for improvement
  
  Using some permanent storage. Good option could be MongoDB. 
  It can store a lot of unstructured JSON files and can be scaled.
  API it self can be horizontally scaled and load balancer can be put in front of them.