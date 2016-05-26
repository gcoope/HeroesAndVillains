<html>
	<head>
	<script src="jquery-1.12.4.min.js"></script>
	<script type="text/javascript">	
	$(document).ready(function(){  		
		ReloadData();
	});
	
	function ReloadData(){
		console.log("called");
		var dataExists = false;
		var scoresDiv = $("#scores_data");
		$.getJSON( "scores.json", function(data) {	
			dataExists = true;
			scoresDiv.empty();
			$.each(data, function(index, players){
				for(var i = 0; i < players.length; i++) {
					scoresDiv.append('<p>' + players[i].playerName + ', ' + players[i].playerTeam + ', ' + players[i].playerScore + '</p>');
				}
			});			
		});   
	   
	   if(!dataExists) {
		scoresDiv.empty();
	   }	   
	   //setTimeout(ReloadData, 3000);
	}	
	</script>
	
	</head>		
	<body>	
	<div id="scores_data"></div>
	<?php
	$hashcode = "smoothstudio123";

	if($_POST["hashcode"] == $hashcode) {
		echo '<script>alert("tits");</script>';
		$json = $_POST["playerPackets"]; // I send through an array of every player
		if($json) {
			// Convert it into a format we can use
			$decodedJson = json_decode($json, true);	

			// Here I save the json file to a text file so we can use it anywhere easily
			if(file_put_contents('scores.json', $json)) {				
				print('<script>ReloadData();</script>');
			}
		}
		
		// Just clears the file - this will in turn cause the table to be empty
		if($_POST["message"] == "EndGame") {
			if(file_put_contents('scores.json', '')) {
				print('<script>ReloadData();</script>');		
			}
		}
	}
	?>
	
	

	</body>
</html>

