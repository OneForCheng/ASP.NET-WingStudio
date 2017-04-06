$(document).ready(function() {
	Init();
});


//初始化页面
function Init()
{
	$("nav").height($(document).height());

	// Hide message
	$(".content .message .close").click(function(){
		$(".content .message").fadeOut(500);
	});

	// Mobile navigation
	var toggleFlag = false;
	var clickFlag = false;
	$(".btn-menu").click(function(){
		clickFlag = true;
		if(toggleFlag)
		{
			$("nav").animate({"left" : "-215px"});
			$("section.content").animate({ marginLeft: 0, marginRight: 0}, 400);
			toggleFlag = false;
		}
		else
		{
			$("nav").animate({"left" : 0}, 20);
			$("section.content").animate({ marginLeft: 215, marginRight: -215}, 20);
			toggleFlag = true;
		}
	});

	// Sticky sidebar
	
	$(window).bind("load resize", function(){
		if(clickFlag)
		{
			$("nav").removeAttr("style");
			$("nav").height($(document).height());
			$("section.content").removeAttr("style");
			clickFlag = false;
		}
	 });

}
