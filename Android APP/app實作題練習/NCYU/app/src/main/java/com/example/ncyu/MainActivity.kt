package com.example.ncyu

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import kotlin.text.toIntOrNull
import android.text.Editable
import android.text.TextWatcher

data class CentennialProduct(
    val name: String,
    val price: Int,
    var quantity: Int = 0 // 預設數量為 0
)

class CentennialProductAdapter(private val products: List<CentennialProduct>) :
    RecyclerView.Adapter<CentennialProductAdapter.ViewHolder>() {

    inner class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val productNameTextView: TextView = itemView.findViewById(R.id.productNameTextView)
        val productPriceTextView: TextView = itemView.findViewById(R.id.productPriceTextView)
        val quantityEditText: EditText = itemView.findViewById(R.id.quantityEditText)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_centennial_product, parent, false)
        return ViewHolder(view)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val product = products[position]
        holder.productNameTextView.text = product.name
        holder.productPriceTextView.text = "$${product.price}"
        holder.quantityEditText.setText(product.quantity.toString())

        holder.quantityEditText.addTextChangedListener(object : TextWatcher {
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}

            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {}

            override fun afterTextChanged(s: Editable?) {
                val quantity = s.toString().toIntOrNull() ?: 0
                product.quantity = quantity
            }
        })
    }

    override fun getItemCount(): Int {
        return products.size
    }
}

class MainActivity : AppCompatActivity() {

    private lateinit var recyclerView: RecyclerView
    private lateinit var calculateButton: Button
    private lateinit var totalTextView: TextView

    private val products = listOf(
        CentennialProduct("百年時尚絲巾", 399),
        CentennialProduct("百年馬克杯", 199),
        CentennialProduct("NCYU紀念帽", 350),
        CentennialProduct("嘉大薄鹽醬油禮盒", 399)
    )

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        recyclerView = findViewById(R.id.recyclerView)
        calculateButton = findViewById(R.id.calculateButton)
        totalTextView = findViewById(R.id.totalTextView)

        recyclerView.layoutManager = LinearLayoutManager(this)
        recyclerView.adapter = CentennialProductAdapter(products)

        calculateButton.setOnClickListener {
            val total = products.sumOf { it.price * it.quantity }
            totalTextView.text = "總金額: $$total"
        }
    }
}