module Index

open Giraffe.GiraffeViewEngine

let index =
    [
        div [_class "row section-head"][
            div [_class "twelve columns"][
                h1 [] [encodedText "Favorite links"]
            ]            
        ]
        div [_class "row add-bottom"][
            hr []
            div [_class "twelve columns add-bottom"][
                p [] [
                    a [_href "#"] [
                        img [_width "120"; _height "120"; _class "pull-left"; _alt "sample-image"; _src "images/sample-image.jpg"]
                        encodedText "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec libero. Suspendisse bibendum.
              			   Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu
              			   posuere nunc justo tempus leo. Donec mattis, purus nec placerat bibendum, dui pede condimentum
              			   odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra
              			   condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque.
              			   posuere nunc justo tempus leo." 
                    ]
                ]
            ]
        ]
        div [_class "row add-bottom"][
            hr []
            div [_class "twelve columns add-bottom"][
                p [] [
                    a [_href "#"] [
                        img [_width "120"; _height "120"; _class "pull-left"; _alt "sample-image"; _src "images/sample-image.jpg"]
                        encodedText "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec libero. Suspendisse bibendum.
              			   Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu
              			   posuere nunc justo tempus leo. Donec mattis, purus nec placerat bibendum, dui pede condimentum
              			   odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra
              			   condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque.
              			   posuere nunc justo tempus leo." 
                    ]
                ]
            ]
        ]
    ]

let layout =
    App.layout index