module Index

open Bolero.Html

let index =
    [
        div [attr.``class`` "row section-head"][
            div [attr.``class`` "twelve columns"][
                h1 [] [text "Favorite links"]
            ]            
        ]
        div [attr.``class`` "row add-bottom"][
            hr []
            div [attr.``class`` "twelve columns add-bottom"][
                p [] [
                    a [attr.href "#"] [
                        img [attr.width "120"; attr.height "120"; attr.``class`` "pull-left"; attr.alt "sample-image"; attr.src "images/sample-image.jpg"]
                        text "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec libero. Suspendisse bibendum.
              			   Cras id urna. Morbi tincidunt, orci ac convallis aliquam, lectus turpis varius lorem, eu
              			   posuere nunc justo tempus leo. Donec mattis, purus nec placerat bibendum, dui pede condimentum
              			   odio, ac blandit ante orci ut diam. Cras fringilla magna. Phasellus suscipit, leo a pharetra
              			   condimentum, lorem tellus eleifend magna, eget fringilla velit magna id neque.
              			   posuere nunc justo tempus leo." 
                    ]
                ]
            ]
        ]
        div [attr.``class`` "row add-bottom"][
            hr []
            div [attr.``class`` "twelve columns add-bottom"][
                p [] [
                    a [attr.href "#"] [
                        img [attr.width "120"; attr.height "120"; attr.``class`` "pull-left"; attr.alt "sample-image"; attr.src "images/sample-image.jpg"]
                        text "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec libero. Suspendisse bibendum.
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

let layout isAdmin =
    App.layout index [] isAdmin