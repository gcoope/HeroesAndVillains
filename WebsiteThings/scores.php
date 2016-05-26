<!DOCTYPE html>
 <html>
 <body>
 
 
<?php

// Connection to Database --------------------------------------------
$dbHost = "localhost"; // Host address here
$dbUser = "root";
$dbPassword = "password"; // Please don't let this be uploaded to git! Or change me if you do
$dbName = "gameScores"; // change to whatever the db is called

$phpHash = "hashcode"; // This much match the one in the games code - done to add a little bit of security from outsiders

// TODO Uncomment lines when database is ready
/*
$connection = mysql_connect($dbHost, dbUser, dbPassword) or die("Unable to connect to database");
$retrieved = mysql_select_db($dbName) or die("Unable to load or find database" + $dbName);
$dbResult = mysqli_query($conection, "SELECT * FROM " + $retrieved);

while($row = mysqli_fetch_array($dbResult)){
	echo "<div><p>" + $row["NAME"] + "</p></div>";
}
 mysqli_close($connection);
*/

echo("<div>hello world</div>");
echo("<div>" + date('m') + ":" + date('s') + "</div>");

// Game functions ----------------------------------
$messageFromGame = $_POST["message"];
if($messageFromGame) {
	switch($messageFromGame) {
		case "StartNewGame":
			StartNewGame();
			break;
			
		case "EndCurrentGame":
			EndCurrentGame();
			break;
			
		case "AddScoreToCurrentGame":
			// We Presume these POST were also sent from the game
			$uniqueID = $_POST["uniqueID"];
			$team = $_POST["team"];
			$amount = $_POST["amount"];
			AddScoreToCurrentGame($uniqueID, $team, $amount);
			break;
			
		default:
			break;
	}
}

function StartNewGame() {
	echo "Starting new game";
	// This will add a new game instance to the database 
	// As there's only ever one game (for now) we could also clear/end the previous game here?
}

function EndCurrentGame() {
	echo "Ending current game";
	// Clears current game so no more info is posted - this is called when game server closes down
}

function AddScoreToCurrentGame($uniqueID, $team, $amount) {    
	// $team can be "hero" or "villain"
	// $amount will be the amount to increment that teams score by - probably always going to be 1
    
    // If we can get current game
    // add score
}

// $uniqueID is a per-player ID that we can use to associate names, teams and scores with

function AddPlayer($uniqueID, $name, $team){

}

function RemovePlayer($uniqueID) {

}

function ChangePlayerName($uniqueID, $newName){

}

function ChangePlayerTeam($uniqueID, $newTeam) {

}

?>
</body>
</html>