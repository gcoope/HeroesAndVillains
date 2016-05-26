<?php

$hashcode = "smoothstudio123";

if($_POST["hashcode"] == $hashcode) {
	
	$json = $_POST["playerPackets"]; // I send through an array of every player
	if($json) {
		// Convert it into a format we can use
		$decodedJson = json_decode($json, true);	

		// Here I save the json file to a text file so we can use it anywhere easily
		if(file_put_contents('scores.json', $json)) {
			echo "Saved to file";		
		}
	}
	
	// Just clears the file so - this will in turn cause the table to be empty
	if($_POST["message"] == "EndGame") {
		if(file_put_contents('scores.json', '')) {
			echo "Clear scores.json";		
		}
	}
}

?>