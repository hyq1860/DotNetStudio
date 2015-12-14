/**
 * Created by feimao on 15-12-8.
 */
$.getJSON('../Scripts/json/recipeData.json', function (obj) {
    var name="孜然羊肉片";
    showTitle(obj[name]);
    showHeader(obj[name]);
    showFoodList(obj[name]);
    showMakeList(obj[name]);
    showTips(obj[name]);
});

function showTitle(menu){
    $("title").text(menu.name+'的做法')
}

function showHeader(menu){
    $(".img").css("background-image","url("+menu.img_url+")");
    $(".menu-name").text(menu.name);
    $(".introduction").text(menu.introduction);
}

function showFoodList(menu){
    menu.food.forEach(function(value){
        $(".food-list ul").append("<li><span class='food-name'>"+value.food_name+"</span>" +
            "<span class='food-note'>"+value.food_note+"</span></li>");
    });
}

function showMakeList(menu){
    menu.make.forEach(function(value,index){
        $(".make-list ul").append("<li><aside style='float: left'>"+(index+1)+"</aside>" +
            "<div style='overflow: hidden'><p>"+value+"</p></div></li>");
    });
}

function showTips(menu){
    var tipsList='';
    menu.tips.forEach(function(value,index){
        $(".tips p").append((index+1)+"、"+value+"<br>");
    });
}
