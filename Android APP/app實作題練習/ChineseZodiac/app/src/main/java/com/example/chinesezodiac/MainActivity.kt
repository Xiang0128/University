package com.example.chinesezodiac

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.widget.Button
import android.widget.ImageView
import java.util.*

class MainActivity : AppCompatActivity() {

    private lateinit var zodiacImageView: ImageView
    private lateinit var selectZodiacButton: Button
    private val zodiacIcons = arrayOf(
        R.drawable.rat, R.drawable.ox, R.drawable.tiger, R.drawable.rabbit,
        R.drawable.dragon, R.drawable.snake, R.drawable.horse, R.drawable.goat,
        R.drawable.monkey, R.drawable.rooster, R.drawable.dog, R.drawable.pork
    )
    private var currentZodiacIndex = 0
    private val handler = Handler(Looper.getMainLooper())
    private var isAnimating = false

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        zodiacImageView = findViewById(R.id.zodiacImageView)
        selectZodiacButton = findViewById(R.id.selectZodiacButton)

        selectZodiacButton.setOnClickListener {
            if (!isAnimating) {
                isAnimating = true
                startAnimation()
                handler.postDelayed({
                    stopAnimation()
                    showRandomZodiac()
                }, 5000) // 5 秒後停止動畫
            }
        }
    }

    private fun startAnimation() {
        handler.post(object : Runnable {
            override fun run() {
                zodiacImageView.setImageResource(zodiacIcons[currentZodiacIndex])
                currentZodiacIndex = (currentZodiacIndex + 1) % zodiacIcons.size
                if (isAnimating) {
                    handler.postDelayed(this, 100) // 每 100 毫秒更新一次圖片
                }
            }
        })
    }

    private fun stopAnimation() {
        isAnimating = false
        handler.removeCallbacksAndMessages(null) // 移除所有延遲任務
    }

    private fun showRandomZodiac() {
        val randomIndex = Random().nextInt(zodiacIcons.size)
        zodiacImageView.setImageResource(zodiacIcons[randomIndex])
    }
}