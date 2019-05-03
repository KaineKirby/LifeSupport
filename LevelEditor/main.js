var selectedTool = 0 ;
var lastClick = null ;

var cells = [] ;

var json = {Barrier: [], AlienDog: [], AlienTurret: [], AlienInfantry: [], OxygenTank: []} ;

function createGrid() {
  var table = $("<table></table>") ;

  for (var i = 0 ; i < 36 ; i++) {
    var row = $("<tr></tr>") ;
    cells.push([]) ;
    for (var j = 0 ; j < 64 ; j++) {
      var column = $("<td></td>") ;

      cells[i].push(column) ;
      column.click(onCellClick(i, j)) ;
      row.append(column) ;
    }
    table.append(row) ;
  }

  $("#main").append(table) ;

  updateTable() ;
}

function onCellClick(row, col) {
  return function() {
    if (row == 0 || row == 35 || col == 0 || col == 63)
      return ;
    switch (selectedTool) {
      case 0:
        removeObjectAt(row, col) ;
      break ;
      case 1:
        removeObjectAt(row, col) ;
        addBarrierAt(row, col) ;
      break ;
      case 2:
        removeObjectAt(row, col) ;
        addObjectAt(row, col) ;
      break ;
      case 3:
        removeObjectAt(row, col) ;
        addObjectAt(row, col) ;
      break ;
      case 4:
        removeObjectAt(row, col) ;
        addObjectAt(row, col) ;
      break ;
      case 5:
        removeObjectAt(row, col) ;
        addObjectAt(row, col) ;
      break ;
    }
    updateTable() ;
    $("#out").val(JSON.stringify(json)) ;
  }
}

function onCellEnter(sRow, sCol, eRow, eCol) {
  return function () {
    //set the css class to unbuilt barrier between the range
    //top to bottom
    if (eRow > sRow) {
      for (var i = sRow ; i < eRow ; i++) {
        cells[i][sCol].addClass("barrierunbuilt") ;
      }
    }
    else {
      for (var i = sCol ; i < eCol ; i++) {
        cells[sRow][i].addClass("barrierunbuilt") ;
      }
    }
  }
}

function onCellLeave() {
  return function() {
    for (var row = 0 ; row < 36 ; row++) {
      for (var col = 0 ; col < 64 ; col++) {
        cells[row][col].removeClass("barrierunbuilt") ;
      }
    }
  }
}

//add an object at the row and column
function addObjectAt(row, col) {
  switch (selectedTool) {
    case 1:
      json.Barrier.push({"Row": lastClick.row, "Column": lastClick.col, "BarrierWidth": col-lastClick.col+1, "BarrierHeight": row-lastClick.row+1})
    break ;
    case 2:
      json.AlienTurret.push({"Row": row, "Column": col, "Speed": 0, "Health":  5, "Damage": 1, "Range": 1000, "ShotSpeed": 1000, "RateOfFire": 1}) ;
    break ;
    case 3:
      json.AlienInfantry.push({"Row": row, "Column": col, "Speed": 150, "Health":  3, "Damage": 1, "Range": 1000, "ShotSpeed": 1000, "RateOfFire": 1}) ;
    break ;
    case 4:
      json.AlienDog.push({"Row": row, "Column": col, "Speed": 300, "Health":  3, "Damage": 1}) ;
    break ;
    case 5:
      json.OxygenTank.push({"Row":  row, "Column": col}) ;
    break ;
  }

}

//remove an object at that row and column if it exists in the json
function removeObjectAt(row, col) {
  for (var i = 0 ; i < json.Barrier.length ; i++) {
    if (json.Barrier[i].Row == row && json.Barrier[i].Column == col) {
      json.Barrier.splice(i, 1) ;
      cells[row][col].removeClass('barrier') ;
    }
  }
  for (var i = 0 ; i < json.AlienDog.length ; i++) {
    if (json.AlienDog[i].Row == row && json.AlienDog[i].Column == col) {
      json.AlienDog.splice(i, 1) ;
      cells[row][col].removeClass('dog') ;
    }
  }
  for (var i = 0 ; i < json.AlienTurret.length ; i++) {
    if (json.AlienTurret[i].Row == row && json.AlienTurret[i].Column == col) {
      json.AlienTurret.splice(i, 1) ;
      cells[row][col].removeClass('turret') ;
    }
  }
  for (var i = 0 ; i < json.AlienInfantry.length ; i++) {
    if (json.AlienInfantry[i].Row == row && json.AlienInfantry[i].Column == col) {
      json.AlienInfantry.splice(i, 1) ;
      cells[row][col].removeClass('inf') ;
    }
  }
  for (var i = 0 ; i < json.OxygenTank.length ; i++) {
    if (json.OxygenTank[i].Row == row && json.OxygenTank[i].Column == col) {
      json.OxygenTank.splice(i, 1) ;
      cells[row][col].removeClass('oxy') ;
    }
  }
}

//function to update the table to match the current json object
function updateTable() {

  for (var i = 0 ; i < 36 ; i++) {
    for (var j = 0 ; j < 64 ; j++) {
      cells[i][j].removeClass("barrier") ;
      cells[i][j].removeClass("turret") ;
      cells[i][j].removeClass("dog") ;
      cells[i][j].removeClass("inf") ;
      cells[i][j].removeClass("oxy") ;

      if (i == 0 || i == 35 || j == 0 || j == 63)
        cells[i][j].addClass('barrier') ;

      if (i == 0 && (j == 31 || j == 32))
          cells[i][j].addClass('door') ;
      if (i == 35 && (j == 31 || j == 32))
        cells[i][j].addClass('door') ;
      if (j == 0 && (i == 17 || i == 18))
          cells[i][j].addClass('door') ;
      if (j == 63 && (i == 17 || i == 18))
          cells[i][j].addClass('door') ;
    }
  }

  for (var i = 0 ; i < json.Barrier.length ; i++) {
    //tall piece
    if (json.Barrier[i].BarrierHeight > json.Barrier[i].BarrierWidth) {
      var count = 0 ;
      while (count < json.Barrier[i].BarrierHeight) {
        cells[json.Barrier[i].Row+count][json.Barrier[i].Column].addClass('barrier') ;
        count++ ;
      }
    }
    //long piece
    else {
      var count = 0 ;
      while (count < json.Barrier[i].BarrierWidth) {
        cells[json.Barrier[i].Row][json.Barrier[i].Column+count].addClass('barrier') ;
        count++ ;
      }
    }

  }
  for (var i = 0 ; i < json.AlienDog.length ; i++) {
    cells[json.AlienDog[i].Row][json.AlienDog[i].Column].addClass('dog') ;
  }
  for (var i = 0 ; i < json.AlienTurret.length ; i++) {
    cells[json.AlienTurret[i].Row][json.AlienTurret[i].Column].addClass('turret') ;
  }
  for (var i = 0 ; i < json.AlienInfantry.length ; i++) {
    cells[json.AlienInfantry[i].Row][json.AlienInfantry[i].Column].addClass('inf') ;
  }
  for (var i = 0 ; i < json.OxygenTank.length ; i++) {
    cells[json.OxygenTank[i].Row][json.OxygenTank[i].Column].addClass('oxy') ;
  }

}

//handles barrier placement operations which are more complicated than standard objects
function addBarrierAt(row, col) {
  //if last click is null, then it is our first click
  if (lastClick == null) {
    lastClick = { row, col } ;
    for (var i = row ; i < 36 ; i++) {
      cells[i][col].mouseenter(onCellEnter(row, col, i, col)) ;
      cells[i][col].mouseleave(onCellLeave()) ;
    }
    for (var i = col ; i < 64 ; i++) {
      cells[row][i].mouseenter(onCellEnter(row, col, row, i)) ;
      cells[row][i].mouseleave(onCellLeave()) ;
    }
  }
  //create the barrier with the math and all that
  else {
    if ((row > lastClick.row && col == lastClick.col) || (col > lastClick.col && row == lastClick.row)) {
      addObjectAt(row, col) ;
    }
    else {
      window.alert("You must define barriers from left to right and from top to bottom!") ;
    }
    lastClick = null ;
    for (var i = 0 ; i < 36 ; i++) {
      for (var j = 0 ; j < 64 ; j++) {
        cells[i][j].unbind("mouseenter") ;
        cells[i][j].unbind("mouseleave") ;
      }
    }
    onCellLeave()() ;
  }
}

function switchTool(tool) {
  selectedTool = tool ;
  switch (tool) {
    case 0:
      $("#controls").css("background-color", "white") ;
    break ;
    case 1:
      $("#controls").css("background-color", "black") ;
    break ;
    case 2:
      $("#controls").css("background-color", "red") ;
    break ;
    case 3:
      $("#controls").css("background-color", "green") ;
    break ;
    case 4:
      $("#controls").css("background-color", "blue") ;
    break ;
    case 5:
      $("#controls").css("background-color", "cyan") ;
    break ;
  }
}

function upload() {
  var text = $("#out").val() ;
  json = JSON.parse(text) ;
  updateTable() ;
}

$(function() {
  createGrid() ;

}) ;
