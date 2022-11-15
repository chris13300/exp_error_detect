# exp_error_detect
Tool to detect and fix errors of experience data from an "eman-like" EXP file<p>

During learning, the engine sometimes generates this kind of error :<br>
![error sample](https://user-images.githubusercontent.com/54830918/202002037-e74c484b-489c-4d68-b623-dfc34ebd11c1.jpg)<p>

Fortunately, we can fix them all automatically :<br>
![exp_error_detect](https://user-images.githubusercontent.com/54830918/202002423-4a9aff78-d2f3-4090-817a-78b10332f84d.jpg)<p>

Once the error is corrected, here is what we get :<br>
![fixed error](https://user-images.githubusercontent.com/54830918/202002780-579b2bb8-6f65-41f5-b792-5a010e6914ce.jpg)<p>

How it work ?<p>
Initially, the program will only search for positions where there are 3 or more moves with the same count value (which must be greater than or equal to the minimum threshold). Then the program will replace the value of the visit counter by 1.<p>

command : exp_error_detect.exe your_experience_file.exp<br>
Configure the minimum threshold for the count value.<br>
