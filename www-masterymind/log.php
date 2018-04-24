<?php

// TOKEN = gSBlq5kjt7ZyqYlIgogSl
// HASH  = 534608680933bc8ebf324a0ed7d9188a

// Check the provided token for validity
$correct_hash = "534608680933bc8ebf324a0ed7d9188a";
$token = htmlspecialchars($_POST['token']);
$hash = hash("md5",$token);
$hash_check = ($hash==$correct_hash);
if ( !$hash_check )
	die("Hash check failed.");

// Connect to database
$db_servername = "localhost";
$db_username = "masterymind";
$db_password = "b452Pi1Enyezip6";
$db_name = "masterymind";
$db_connection = new mysqli($db_servername,$db_username,$db_password);
if ( $db_connection->connect_error )
	die("Database connection failed.");
if ( !mysqli_select_db($db_connection,$db_name) )
	die("Database opening failed.".mysqli_error($db_connection));

// Fetch information
$log_timestamp = htmlspecialchars($_POST['timestamp']);
$log_userid = htmlspecialchars($_POST['userid']);
$log_event = htmlspecialchars($_POST['event']);
$log_param1 = htmlspecialchars($_POST['param1']);
$log_param2 = htmlspecialchars($_POST['param2']);
$log_param3 = htmlspecialchars($_POST['param3']);
$log_param4 = htmlspecialchars($_POST['param4']);
$log_param5 = htmlspecialchars($_POST['param5']);

// Add to database
$query = "INSERT INTO `logentries` (`id`, `timestamp`, `userid`, `event`, `param1`, `param2`, `param3`, `param4`, `param5`) VALUES 
	(NULL, $log_timestamp, '$log_userid', '$log_event', $log_param1, $log_param2, $log_param3, '$log_param4', $log_param5);";
if ( !mysqli_query($db_connection,$query) )
	die(mysqli_error($db_connection));

?>