package com.example.phone

import android.content.Intent
import android.net.Uri
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val callButton = findViewById<Button>(R.id.callButton)
        val openWebButton = findViewById<Button>(R.id.openWebButton)

        callButton.setOnClickListener {
            val phoneNumber = "052732906"
            val intent = Intent(Intent.ACTION_DIAL, Uri.parse("tel:$phoneNumber"))
            startActivity(intent)
        }

        openWebButton.setOnClickListener {
            val webUrl = "https://misweb.ncyu.edu.tw/teachers/tuliang/tuliang.html"
            val intent = Intent(Intent.ACTION_VIEW, Uri.parse(webUrl))
            startActivity(intent)
        }
    }
}